//Predefined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Added
using System.Diagnostics;
using System.Text.RegularExpressions;

/* Program by Max Arnheiter
 * 
 * The goal of this learning project was to use WPF for the first time, and combine it with some of the event 
 * knowlege I've gained, in order to visually illustrate one implementation of the Undo and Redo concepts. 
 * 
 * The program works, and my code is relatively neat. However, this projec thas left me feeling a bit concerned: 
 * 
 *      1. I feel like there are many ways to implement Undo/Redo. 
 *          - I could have implemented the stacks as their own objects, and have tied all of the controls and 
 *            events to those objects. 
 *          - I could have handled the text updating differently. I could have simply stored all of the event args
 *            information, and then when a change occured, run through all of them to create the final text output. 
 *            I didnt do this because I believe it would be unwise when working with a large body of text. 
 *          - There seems to be a built-in undo functionallity in WPF(?) or some of its components. It would have 
 *            been interesting to get that working. I might still do that. 
 *            
 *      2. I'm dissatisfied with my use of events here. 
 *          - Events from the UI come in as OnSomething and thats fine becuase theyre actually events. All of the 
 *            other functions I've used in this project use the On prefix, despite not being linked to any kind 
 *            of event or delegate. I could have changed this, but the extra functionallity wasn't necessary. The 
 *            end result would have been the same. 
 *      
 * 
 * All-in-all, it was a nice learning endevour, and I had a lot of fun working with WPF for the first time. Making 
 * my own custom event args, and storing information in a reversable object is pretty cool. 
 * */


namespace WPF_Undo_Redo
{
    
    public partial class MainWindow : Window
    {
        //We don't want to trigger OnTextChanged when working internally, so we have a bool to 'skip' the next text change. 
        bool _ignoreTextChange = true;   
   
        //We also don't want to trigger OnTextChanged when the user initially clicks on the text area. 
        bool _initialTextClear = true;

        //Its going to be important to keep the state of the text box before it was changed.
        string _previousText;

        Stack<CustomChangeArgs> _pastEvents;    
        Stack<CustomChangeArgs> _futureEvents;


        public MainWindow()
        {
            //Initialize our stacks
            _pastEvents = new Stack<CustomChangeArgs>();
            _futureEvents = new Stack<CustomChangeArgs>();

            InitializeComponent();
        }

        //This is just a nice way of directing the user to the input area. It requires 
        //a little code on our part, but it looks professional. 
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (_initialTextClear)
            {
                MainTextBox.Text = "";
                _previousText = MainTextBox.Text;
                _initialTextClear = false;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.IsInitialized)
            if(this._ignoreTextChange)              //This lets us 'skip' text changes
            {
                this._ignoreTextChange = false;
                return;
            }
            else
            {
                foreach(TextChange tC in e.Changes) //Could also be e.Changes.First()
                {
                    string addedChars = null;
                    string removedChars = null;

                    //Check to see if we added any characters
                    if(tC.AddedLength > 0)
                    {
                        addedChars = MainTextBox.Text.Substring(tC.Offset, tC.AddedLength);
                    }
                    //Check to see if we removed any characters
                    if(tC.RemovedLength > 0)
                    {
                        removedChars = _previousText.Substring(tC.Offset, tC.RemovedLength);
                    }
                    //Create a new object to hold our event info, and pass it on
                    OnPastEventsPush(new CustomChangeArgs(tC.Offset, tC.AddedLength, tC.RemovedLength, addedChars, removedChars), true);
                }
            }
            _previousText = MainTextBox.Text;
        }

        //This is called when an event is entering the past events stack
        private void OnPastEventsPush(CustomChangeArgs args, bool isNew)
        {
            _pastEvents.Push(args);
            if (isNew)                      //We use a bool to determine if the event is new (user hit a key, not undo/redo)
                _futureEvents.Clear();      //If its a new input event, we need to clear the future events stack since
                                            //we've branched off from our previous timeline.

            OnStackChange();                
        }

        //Ths is called when an event is leaving the past events stack (aka Undo)
        private void OnPastEventsPop()
        {
            CustomChangeArgs args = _pastEvents.Pop();          //Grab the last event
            MainTextBox.Text = args.Undo(MainTextBox.Text);     //Apply it to our text
            OnFutureEventsPush(args);                           //Push it onto the future events stack

            OnStackChange();                                    
        }

        //This is called when an event enters the future events stack (aka Undo)
        private void OnFutureEventsPush(CustomChangeArgs args)
        {
            _futureEvents.Push(args);       //Just add the event to the stack and update
            OnStackChange();
        }

        //this is called when an event is leaving the future events stack (aka Redo)
        private void OnFutureEventsPop()
        {
            CustomChangeArgs args = _futureEvents.Pop();        //Pop the last event from the future events stack
            MainTextBox.Text = args.Redo(MainTextBox.Text);     //Apply the event to our text
            OnPastEventsPush(args, false);                      //Push the event back to the past events stack

            OnStackChange();
        }

        //Every time we make some kind of a change to either stack, we need to do a series of UI updates.
        //These updates are handled here, in one section, for easy readability. They could, and probably should, 
        //all be in their own methods. 
        private void OnStackChange()
        {
            //Update Text Blocks - this could be in its own function.
            PastEventsTextBlock.Text = Regex.Replace(PastEventsTextBlock.Text, @"\d", "");      //Regex is just for replacing
            PastEventsTextBlock.Text += _pastEvents.Count;                                      //the number in the text.

            FutureEventsTextBlock.Text = Regex.Replace(FutureEventsTextBlock.Text, @"\d", "");
            FutureEventsTextBlock.Text += _futureEvents.Count;

            //Update Stack Panels - this could be in its own function.
            PastEventsStackPanel.Children.Clear();
            foreach (CustomChangeArgs changeArgs in _pastEvents)
                PastEventsStackPanel.Children.Add(new Button { Content = changeArgs.ToString() });

            FutureEventsStackPanel.Children.Clear();
            foreach (CustomChangeArgs changeArgs in _futureEvents)
                FutureEventsStackPanel.Children.Add(new Button { Content = changeArgs.ToString() });

            //Update Buttons - this could be in its own function.
            if (_pastEvents.Count == 0)                           //All we do here is enable and disable depending
                UndoButton.IsEnabled = false;                     //on whether or not the operation in question can 
            else                                                  //even be executed. (Can't undo if you don't have past events)
                UndoButton.IsEnabled = true;

            if (_futureEvents.Count == 0)
                RedoButton.IsEnabled = false;
            else
                RedoButton.IsEnabled = true;
    
        }

        private void OnUndo(object sender, RoutedEventArgs e)
        {
            if (_pastEvents.Count > 0)
            {
                this._ignoreTextChange = true;
                OnPastEventsPop();
            }
        }

        private void OnRedo(object sender, RoutedEventArgs e)
        {
            if(_futureEvents.Count > 0)
            {
                this._ignoreTextChange = true;
                OnFutureEventsPop();
            }
        }
      
    }
}
