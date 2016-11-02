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
using System.IO;
using System.Windows.Forms;
using System.Threading;
using SimpletonUnitTestFramework;

namespace ImageViewer
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		string[] images;
		int currentImage = 0;
		Keys pressedKey;
		
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
		    
			DisplayImage(images, currentImage);

		}

		void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
		{
			//When scrolling up on picturebox
			if(e.Delta > 0) {
				
				//For testing purposes
				var previousImage = currentImage;
				
				//Try previous image
				currentImage--;
				
				//if scroll reverse past image start, go last image
				currentImage = Wrap(0, images.Length-1, currentImage);	
				
				DisplayImage(images, currentImage);
				
				tester.Assert("picturebox scrolls to next image", currentImage +1, previousImage);
			}
			
			//When holding CTRL + scrolling up on picturebox
			if ((pressedKey.Equals(Keys.ControlKey)) && (e.Delta > 0)) {
				
				//zoom in
				
				
			}
			
			//When scrolling down on picturebox
			if(e.Delta < 0) {
				
				//try next image
				currentImage++;

				//if scroll forward past image end, go back to first image
				currentImage = Wrap(0,images.Length, currentImage);
				
				DisplayImage(images, currentImage);
			}
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
