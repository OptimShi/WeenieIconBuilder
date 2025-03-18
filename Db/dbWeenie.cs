using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WeenieIconBuilder.Enums;

namespace WeenieIconBuilder.Db
{
    public class dbWeenie
    {
        public int WCID;
        public Dictionary<PropertyDID, int> DIDs;
        public Dictionary<PropertyInt, int> Ints;
        public Dictionary<PropertyBool, bool> Bools;
    }
}
