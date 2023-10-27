using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculatorApp
{
    public class History                            // Реалізвувати патерн Memory
    {
        private LinkedList<string> history = new LinkedList<string>();

        public string[] Records
        {
            get
            {
                return history.ToArray();
            }
        }

        public void AddRecord(string record)
        {
            history.AddLast(record);
        }
        public string LastRecord()
        {
            if (history.Count > 0)
                return history.Last.Value;

            return string.Empty;
        }
        public void RemoveRecord(string record)
        {
            history.Remove(record);
        }
        public void Clear()
        { 
            history.Clear();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (string item in history)
            {
                result.AppendLine(item);
            }

            return result.ToString();
        }
    }
}
