using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Reflection;

using System.Runtime.Serialization.Formatters.Binary;


namespace NYP.EVoting
{
   [Serializable]
    public class Party 
    {
        private string _constituencyPartyMappingid;
        private String _partyId;
        private String _partyName;
        private byte[] _partySymbol;
        private DateTime _dateOfInception;
        private String _status;
        private Boolean _selected;

        public string ConstituencyPartyMappingID
        {
            get { return _constituencyPartyMappingid; }
            set { _constituencyPartyMappingid = value; }
        }
        public string PartyID
        {
            get { return _partyId; }
            set { _partyId = value; }
        }
        public string PartyName
        {
            get { return _partyName; }
            set { _partyName = value; }
        }
        public byte[] PartySymbol
        {
            get { return _partySymbol; }
            set { _partySymbol = value; }
        }
        public DateTime DateOfInception
        {
            get { return _dateOfInception; }
            set { _dateOfInception = value; }
        }
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public Boolean Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }
    }
}
