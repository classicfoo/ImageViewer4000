/*
 * Created by SharpDevelop.
 * User: mike
 * Date: 30/10/2016
 * Time: 1:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using SimpletonUnitTestFramework;

namespace ImageViewer
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		string[] images;
		int currentImageIndex = 0;
		Keys pressedKey;
		double zoomFactor;
		Bitmap workingImage;
		
		//Enable unit testing
		SimpletonUnitTestFramework.MainForm tester;
		
		public MainForm(string[] args)
		{
			//After all the widgets load ...
			InitializeComponent();
			
			//Initialize tester ...
			tester = new SimpletonUnitTestFramework.MainForm();
			tester.Show();
			
			//Register the events ...
			this.pictureBox1.MouseEnter += new System.EventHandler(this.PictureBox1_MouseEnter);
			this.pictureBox1.MouseWheel += new MouseEventHandler(PictureBox1_MouseWheel);
			
			//Get image files in dir
			var filters = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp" };
			var searchFolder = ".";
			var isRecursive = false;
			List<String> filesFound = new List<String>();
			var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			foreach (var filter in filters) {
				filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
			}
			images = filesFound.ToArray();
			
			tester.Assert("all images are loaded", 3, images.Length);
			
			
			workingImage = (Bitmap)Image.FromFile(images[currentImageIndex]);
			
			DisplayImage(images, currentImageIndex);

		}

		void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
		{
			//When scrolling up on picturebox
			//go to previous image
			if(e.Delta > 0) {
				
				//For testing purposes
				var previousImage = currentImageIndex;
				
				//Try previous image
				currentImageIndex--;
				
				//if scroll reverse past image start, go last image
				currentImageIndex = Wrap(0, images.Length-1, currentImageIndex);
				
				DisplayImage(images, currentImageIndex);
				
				tester.Assert("picturebox scrolls to next image", currentImageIndex +1, previousImage);
				
			}
			
			//When scrolling down on picturebox
			//go to next image
			if(e.Delta < 0) {
				
				//try next image
				currentImageIndex++;

				//if scroll forward past image end, go back to first image
				currentImageIndex = Wrap(0,images.Length, currentImageIndex);
				
				DisplayImage(images, currentImageIndex);
			}
			
//			//When holding CTRL + scrolling up on picturebox
//			if ((pressedKey.Equals(Keys.ControlKey)) && (e.Delta > 0)) {
//				
//				//zoom in
//				zoomFactor += 0.1;
//				
//
//				Bitmap resizedImage = ResizeImage(workingImage, zoomFactor);
//				Rectangle rectangle = new Rectangle(pictureBox1.Left, pictureBox1.Top,pictureBox1.Width, pictureBox1.Height);
//				
//				
//				tester.Print(string.Format("Rectangle x:{0} y:{1} width:{2} height:{3}",rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height));
//				
//				
//				workingImage = CropImage(resizedImage, rectangle);
//				pictureBox1.Image = workingImage;
//				
//			}
//			
//			//When holding CTRL + scrolling down on picturebox
//			if ((pressedKey.Equals(Keys.ControlKey)) && (e.Delta < 0)) {
//				
//				//zoom out
//				string currentImage = images[currentImageIndex];
//				
//
//				zoomFactor += 0.1;
//				pictureBox1.Image = ResizeImage(currentImage, zoomFactor);
//
//				
//			}
			
			//test mouse scroll speed is always consistent
			//tester.Assert("Mouse scroll speed is consistent", (Math.Abs(e.Delta)).ToString(), "120");
			
				

		}
		
		//resizing large images is really slow
		Bitmap ResizeImage(Bitmap workingImage, double zoomFactor)
		{
			//Bitmap original = (Bitmap)Image.FromFile(imageLocation);
			
			Size size = new Size((int)(workingImage.Width * (1+zoomFactor)),(int)(workingImage.Height * (1+zoomFactor)));
			Bitmap resizedImage = new Bitmap(workingImage, size);
			
			// discovered that error occurs when zoomDelta is 0
			tester.Print("zoomFactor: " + zoomFactor);
			
			return resizedImage;
		}
		
		Bitmap CropImage(Bitmap bitmap, Rectangle r)
		{
			//return null;
		   	//return bitmap.Clone(cropArea, bitmap.PixelFormat);
		   	Graphics g = Graphics.FromImage(bitmap);
		   	g.DrawImage(bitmap, -r.X, -r.Y);
			return bitmap;

		}
		
		//increment end to beginning and vice versa (super mario bros rules)
		public int Wrap(int start, int end, int current)
		{
			if(current > end)		{ return start; }
			else if(current < 0)	{ return end; }
			else 					{ return current; }
		}
		
		//show an image in picturebox
		void DisplayImage(string[] images, int imageIndex)
		{
			//prevent stuff up when scrolling too fast
			if(imageIndex <= images.Length -1 && imageIndex >= 0) {
				pictureBox1.ImageLocation = images[imageIndex];
				this.Text = "ImageViewer4000 - ";
				this.Text += Path.GetFileName(images[imageIndex]);
			}
		}
		
		//enable mousemove events
		void PictureBox1_MouseEnter(object sender, EventArgs e)
		{
			pictureBox1.Focus();
		}
		
		void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			pressedKey = e.KeyCode;
			
			//tester.Assert("escape is pressed", Keys.Escape, e.KeyCode);
		}

		
	}
}
