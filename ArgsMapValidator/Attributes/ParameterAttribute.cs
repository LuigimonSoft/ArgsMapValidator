using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgsMapValidator.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class ParameterAttribute : Attribute
  {
    public int Index { get; }

    public ParameterAttribute(int index)
    {
      Index = index;
    }
  }
}
