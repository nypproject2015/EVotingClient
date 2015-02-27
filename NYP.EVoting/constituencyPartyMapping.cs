using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;

using System.Runtime.Serialization.Formatters.Binary;


namespace NYP.EVoting
{
   
    public class constituencyPartyMapping
    {
        private string _constituencyID;
        private string _constituencyName;
        private Party[] _partyDesc;
        public constituencyPartyMapping()
        { }
        public string ConstituencyID
        {
            get { return _constituencyID; }
            set { _constituencyID = value; }
        }
        public string ConstituencyName
        {
            get { return _constituencyName; }
            set { _constituencyName = value; }
        }
        public Party[] PartyDescription
        {
            get { return _partyDesc; }
            set { _partyDesc = value; }
        }

      }
}

//getConstituencyName

