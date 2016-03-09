using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IScissors
{
    public class MenuComponent : GameComponent
    {
        public bool HasMouse { get; private set; }
        private bool menuActive;

        public Editor Editor
        {
            get { return editor; }
            set
            {
                editor = value;
                RefreshMenuItems();
                Editor.Scissors.OnPlace += RefreshMenuItems;
            }
        }

        private ToolStripMenuItem actionMenu;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem viewMenu;

        private MenuStrip menuStrip;
        private Blurrer blurrer;
        private Editor editor;

        public MenuComponent(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            var root = Control.FromHandle(Game.Window.Handle);

            menuStrip = new MenuStrip();

            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Save Contour", null, (o, e) =>
            {
                var filename = GetSaveFileName();
                if (filename == null || !filename.EndsWith(".png")) return;

                var contourImage = Editor.Scissors.ImageWithContour();
                var contourTexture = contourImage.ToTexture();

                SaveTexture(contourTexture, filename);
            });
            fileMenu.DropDownItems.Add("Save Mask", null, (o, e) =>
            {
                var filename = GetSaveFileName();
                if (filename == null || !filename.EndsWith(".png")) return;

                var contourImage = Editor.Scissors.Mask();
                var contourTexture = contourImage.ToTexture();

                SaveTexture(contourTexture, filename);
            });
            fileMenu.DropDownItems.Add("Open", null, (o, e) =>
            {
                var filename = GetOpenFileName();
                if (filename == null) return;

                var texture = LoadTexture(filename);
                if (texture == null) return; 
                Editor.Load(texture);
            });
            fileMenu.DropDownItems.Add("Clear", null, (o, e) => Editor.Reset());
            fileMenu.DropDownItems.Add("Exit", null, (o, e) => Environment.Exit(0));
            menuStrip.Items.Add(fileMenu);
            
            editMenu = new ToolStripMenuItem("Edit");
            editMenu.DropDownItems.Add("Undo", null, (o, e) => Editor.Scissors.Undo());
            menuStrip.Items.Add(editMenu);

            viewMenu = new ToolStripMenuItem("View");
            viewMenu.DropDownItems.Add("Reset View", null, (o, e) => Editor.ResetView());
            var zoomMenu = new ToolStripMenuItem("Zoom");
            zoomMenu.DropDownItems.Add("1:2", null, (o, e) => Editor.Zoom = 0.5f);
            zoomMenu.DropDownItems.Add("1:1", null, (o, e) => Editor.Zoom = 1);
            zoomMenu.DropDownItems.Add("1:2", null, (o, e) => Editor.Zoom = 2);
            zoomMenu.DropDownItems.Add("1:4", null, (o, e) => Editor.Zoom = 4);
            viewMenu.DropDownItems.Add(zoomMenu);

            var imageMenu = new ToolStripMenuItem("Image Mode");
            imageMenu.DropDownItems.Add("Default", null, (o,e) => Editor.Scissors.SetImageMode(ImageMode.Default));
            imageMenu.DropDownItems.Add("Costs", null, (o, e) => Editor.Scissors.SetImageMode(ImageMode.Cost));
            imageMenu.DropDownItems.Add("Pixel Nodes", null, (o, e) => Editor.Scissors.SetImageMode(ImageMode.PixelNode));
            imageMenu.DropDownItems.Add("Sobel", null, (o, e) => Editor.Scissors.SetImageMode(ImageMode.Sobel));
            imageMenu.DropDownItems.Add("Path Tree", null, (o, e) => Editor.Scissors.SetImageMode(ImageMode.PathTree));
            viewMenu.DropDownItems.Add(imageMenu);
            menuStrip.Items.Add(viewMenu);

            actionMenu = new ToolStripMenuItem("Action");
            actionMenu.DropDownItems.Add("Start", null, (o, e) =>
            {
                Editor.Scissors.Resume();
                RefreshMenuItems();
            });
            actionMenu.DropDownItems.Add("Stop", null, (o, e) =>
            {
                Editor.Scissors.Pause();
                RefreshMenuItems();
            });
            actionMenu.DropDownItems.Add("Close", null, (o, e) =>
            {
                Editor.Scissors.Close();
                RefreshMenuItems();
            });
            menuStrip.Items.Add(actionMenu);

            var optionsMenu = new ToolStripMenuItem("Options");
            optionsMenu.DropDownItems.Add("Blur", null, (o, e) =>
            {
                blurrer = blurrer ?? new Blurrer()
                {
                    Completed = v => { Editor.Scissors.Blur(v); }
                };
                blurrer.ShowDialog();
            });
            menuStrip.Items.Add(optionsMenu);

            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About", null, (o, e) => MessageBox.Show("Intelligent Scissors. A COMP 5421 Project by Jay Harris", "About"));
            menuStrip.Items.Add(helpMenu);

            root.Controls.Add(menuStrip);

            AddMouseListener(menuStrip);
        }

        public void RefreshMenuItems()
        {
            actionMenu.DropDownItems[0].Enabled = Editor.Scissors.CanStart();
            actionMenu.DropDownItems[1].Enabled = Editor.Scissors.CanStop();
            actionMenu.DropDownItems[2].Enabled = Editor.Scissors.CanClose();

            editMenu.DropDownItems[0].Enabled = Editor.Scissors.CanUndo();
        }

        private void AddMouseListener(MenuStrip control)
        {
            if (control == null)
                return;

            control.MouseEnter += (o, e)=> MouseEntered(o);
            control.MouseLeave += (o, e) =>
            {
                if (menuActive) return;
                MouseExited(o);
            };
            control.MenuActivate += (o, e) =>
            {
                menuActive = true;
                MouseEntered(o);
            };
            control.MenuDeactivate += (o, e) =>
            {
                menuActive = false;
                MouseExited(o);
            };
        }

        private void MouseEntered(object entered)
        {
            HasMouse = true;
            Console.WriteLine($"Has Mouse: {HasMouse}");
        }

        private void MouseExited(object exited)
        {
            HasMouse = false;
            Console.WriteLine($"Has Mouse: {HasMouse}");
        }

        private string GetOpenFileName()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp|Image Files|*.png|Image Files|*.jpg;*.jpeg|Image Files|*.bmp";
            openFileDialog.ShowDialog();

            return openFileDialog.FileName;
        }

        private string GetSaveFileName()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Image Files|*.png";
            saveFileDialog.ShowDialog();
            
            return saveFileDialog.FileName;
        }

        private Texture2D LoadTexture(string filename)
        {
            try
            {
                using (var stream = File.OpenRead(filename))
                {
                    return Texture2D.FromStream(TextureUtil.Device, stream);
                }
            }
            catch
            {
                
            }

            return null;
        }

        private void SaveTexture(Texture2D texture, string filename)
        {
            using (var stream = File.Create(filename))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }
        }
    }

    public enum ImageMode
    {
        Default,
        Cost,
        PixelNode,
        Sobel,
        PathTree
    }
}
