using System;
using System.Collections.Generic;
using System.Linq;

namespace CE.DbUpgrade.Logic
{
    public class SqlScriptCall
    {
        public string Name { get; private set; }
        public string Action { get; private set; }
        public Version CallbackVersion { get; set; }
        public IList<string> Lines { get; private set; }

        //SetMessage("Updating Areas_Sold...")
        //ProcessQueryByNameAndVersion("Areas_Sold", "18.1.48")
        // -- OR --
        //ProcessQueryByName("TimeClock_Breaks")
        public SqlScriptCall(IList<string> lines)
        {
            if (lines.Count() != 2)
                throw new ArgumentException($"Must have 2 lines to parse SqlScriptCall, had {lines.Count()} line(s).");

            Lines = lines;

            // SetMessage("Updating TimeClock_Breaks...")
            var messageLine = lines[0].Trim(); ;
            var messageLineSections = messageLine.Split(' ');

            // SetMessage("Updating
            Action = messageLineSections[0].Replace("SetMessage(\"", "");

            // TimeClock_Breaks...")
            Name = messageLineSections[1].Replace("...\")", "");

            //ProcessQueryByName("TimeClock_Breaks")
            var processLine = lines[1].Trim();

            if (processLine.Contains("ProcessQueryByNameAndVersion"))
            {
                // parse the callback
                //ProcessQueryByNameAndVersion("Areas_Sold", "18.1.48")
                var processLineSections = processLine.Replace("ProcessQueryByNameAndVersion(", "").
                                                        Replace("(", "").
                                                        Replace(")", "").
                                                        Replace("\"", "").
                                                        Split(',');

                CallbackVersion = Version.Parse(processLineSections[1].Trim());

                var expectedLine = String.Format("ProcessQueryByNameAndVersion(\"{0}\", \"{1}\")", Name, CallbackVersion.ToString());

                // Areas_Sold, 18.1.48
                if (processLine.Trim() != expectedLine)
                {
                    throw new ArgumentException($"Process line name, {processLineSections[0].Trim()}, did not match Expected line, {expectedLine}. MessageLine: {messageLine}; ProcessLine: {processLine}"); 
                }

                
            }
            else if (processLine.Contains("ProcessQueryByName"))
            {
                var expectedLine = String.Format("ProcessQueryByName(\"{0}\")", Name);

                if (processLine.Trim() != expectedLine)
                {
                    Name = processLine.Replace("ProcessQueryByName(\"", "").Replace("\")", "");

                    expectedLine = String.Format("ProcessQueryByName(\"{0}\")", Name);

                    if (processLine.Trim() != expectedLine)
                        throw new ArgumentException($"Process line, {processLine}, did not match Expected line, {expectedLine}. MessageLine: {messageLine}; ProcessLine: {processLine}");
                }
            }
            else
            {
                throw new ArgumentException($"Unable to parse Process Line. MessageLine: {messageLine}; ProcessLine: {processLine}");
            }

        }
    }
}
