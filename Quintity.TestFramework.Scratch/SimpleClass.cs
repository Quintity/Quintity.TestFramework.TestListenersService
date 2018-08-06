using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using QTF = Quintity.TestFramework.Core;


namespace Quintity.TestFramework.Scratch
{
    [QTF.TestClass]
    public class SimpleClass : ScratchBase
    {
        //[DataMember]
        public string StringMember
        { get; set; }

        public SimpleClass()
        {
            QTF.TestWarning.IsTrue(1 < 0, "Warning:  {0}", "Bob");
        }
    }
}
