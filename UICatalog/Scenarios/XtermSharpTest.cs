using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Terminal.Gui;
using XtermSharp;

namespace UICatalog {
	[ScenarioMetadata (Name: "XtermSharpTest", Description: "Testing XtermSharp")]
	[ScenarioCategory ("Controls")]
	class XtermSharpTest : Scenario {

		private string _fileName = "demo.an";
		private TerminalView _terminal;
		private bool _saved = true;

		public override void Setup ()
		{
			Win.Title = this.GetName () + "-" + _fileName ?? "Untitled";
			Win.Y = 1; // menu
			Win.Height = Dim.Fill (1); // status bar
			Top.LayoutSubviews ();

			var menu = new MenuBar (new MenuBarItem [] {
				new MenuBarItem ("_File", new MenuItem [] {
					new MenuItem ("_New", "", () => New()),
					new MenuItem ("_Open", "", () => Open()),
					new MenuItem ("_Save", "", () => Save()),
					null,
					new MenuItem ("_Quit", "", () => Quit()),
				}),
				new MenuBarItem ("_Edit", new MenuItem [] {
					new MenuItem ("_Copy", "", () => Copy()),
					new MenuItem ("C_ut", "", () => Cut()),
					new MenuItem ("_Paste", "", () => Paste())
				}),
			});
			Top.Add (menu);

			var statusBar = new StatusBar (new StatusItem [] {
				//new StatusItem(Key.Enter, "~ENTER~ ApplyEdits", () => { _hexView.ApplyEdits(); }),
				new StatusItem(Key.F2, "~F2~ Open", () => Open()),
				new StatusItem(Key.F3, "~F3~ Save", () => Save()),
				new StatusItem(Key.ControlQ, "~^Q~ Quit", () => Quit()),
			});
			Top.Add (statusBar);

			CreateDemoFile (_fileName);

			_terminal = new SubprocessTerminalView () {
				X = 0,
				Y = 0,
				Width = Dim.Fill (),
				Height = Dim.Fill ()
			};

			_terminal.Feed (LoadFile (), -1);

			Win.Add (_terminal);

		}

		private void New ()
		{
			_fileName = null;
			Win.Title = this.GetName () + "-" + _fileName ?? "Untitled";
			throw new NotImplementedException ();
		}

		private byte [] LoadFile ()
		{
			if (!_saved) {
				MessageBox.ErrorQuery ("Not Implemented", "Functionality not yet implemented.", "Ok");
			}

			if (_fileName != null) {
				_terminal.Reset ();
				var bin = System.IO.File.ReadAllBytes (_fileName);
				Win.Title = this.GetName () + "-" + _fileName;
				_saved = true;
				return bin;
			}
			return null;
		}

		private void Paste ()
		{
			MessageBox.ErrorQuery ("Not Implemented", "Functionality not yet implemented.", "Ok");
		}

		private void Cut ()
		{
			MessageBox.ErrorQuery ("Not Implemented", "Functionality not yet implemented.", "Ok");
		}

		private void Copy ()
		{
			MessageBox.ErrorQuery ("Not Implemented", "Functionality not yet implemented.", "Ok");
			//if (_textView != null && _textView.SelectedLength != 0) {
			//	_textView.Copy ();
			//}
		}

		private void Open ()
		{
			var d = new OpenDialog ("Open", "Open a file") { AllowsMultipleSelection = false };
			Application.Run (d);

			if (!d.Canceled) {
				_fileName = d.FilePaths [0];
				_terminal.Feed (LoadFile (), -1);
			}
		}

		private void Save ()
		{
			if (_fileName != null) {
				throw new NotImplementedException ();
				_saved = true;
			}
		}

		private void Quit ()
		{
			Application.RequestStop ();
		}

		private void CreateDemoFile (string fileName)
		{
			var sb = new StringBuilder ();
			// BUGBUG: #279 TextView does not know how to deal with \r\n, only \r
			sb.Append ("Hello world.\n");
			sb.Append ("This is a test of the Emergency Broadcast System.\n");

			var sw = System.IO.File.CreateText (fileName);
			sw.Write (sb.ToString ());
			sw.Close ();
		}
	}
}