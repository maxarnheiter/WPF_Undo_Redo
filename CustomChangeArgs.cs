using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Undo_Redo
{
    class CustomChangeArgs : EventArgs
    {
        //The offset within the text, where the change occured
        int _offset;
        public int Offset
        { 
            get { return _offset; }
            set { _offset = value; }
        }

        //How many characters were added
        int _addedLength;
        public int AddedLength
        {
            get { return _addedLength; }
            set { _addedLength = value; }
        }

        //How many characters were removed
        int _removedLength;
        public int RemovedLength
        {
            get { return _removedLength; }
            set { _removedLength = value; }
        }

        //The characters that were added
        string _addedChars;
        public string AddedChars
        {
            get { return _addedChars; }
            set { _addedChars = value;  }
        }

        //The characters that were removed
        string _removedChars;
        public string RemovedChars
        {
            get { return _removedChars; }
            set { _removedChars = value; }
        }

        //Constructor
        public CustomChangeArgs(int offset, int addedLength, int removedLength, string addedChars, string removedChars)
        {
            this._offset = offset;
            this._addedLength = addedLength;
            this._removedLength = removedLength;
            this._addedChars = addedChars;
            this._removedChars = removedChars;
        }

        //This is just nice to print internal info to buttons.
        public override string ToString()
        {
            return (
                        "(" + this.Offset + "," + this.AddedLength + "," + this.RemovedLength + ")" + 
                        " + " + (this.AddedChars ?? "null") +
                        " - " + (this.RemovedChars ?? "null")
                   );
        }

        public string Redo(string source)                       //Feed in the text, get out the end result
        {
            StringBuilder sb = new StringBuilder(source);

            if(this.RemovedLength > 0)                          //If we've removed anything in the past, then lets undo that
                sb.Remove(this.Offset, this.RemovedLength);
            if (this.AddedLength > 0)                           //Now we need to re-apply the initial changes, and return
                sb.Insert(this.Offset, this.AddedChars);

            return sb.ToString();
        }

        public string Undo(string source)                       //Feed in the text, get out the end result
        {
            StringBuilder sb = new StringBuilder(source);

            if (this.AddedLength > 0)                           //We need to remove the text we've added, first
                sb.Remove(this.Offset, this.AddedLength);
            if (this.RemovedLength > 0)                         //Now lets put back the characters we initially removed and return
                sb.Insert(this.Offset, this.RemovedChars);

            return sb.ToString();
        }
    }
}
