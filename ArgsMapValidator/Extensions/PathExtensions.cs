using System;
using System.IO;

namespace System.IO {
  public static class PathExtensions
  {
    public static bool IsAnyInvalidCharacterInPath(this char[] invalidChars, string path)
    {
      foreach (var ch in path)
      {
        if (Array.IndexOf(invalidChars, ch) >= 0)
          return true;
      }
      return false;
    }
  }
}