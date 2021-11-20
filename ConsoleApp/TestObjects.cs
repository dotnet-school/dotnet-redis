using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace ConsoleApp
{
  public class TestObjects
  {

    public static void Run(IDatabase db)
    {
    }
    
    public static void CheckObject()
    {
      var one = new Person()
      {
              Name = "One",
              Age = 1,
              Salary = 231.34m,
              IsMale = true,
              FavoriteColors = new HashSet<string>(){"red", "blue", "green"},
              PreviousAddresses = new List<string>(){"Delhi", "Mumbai", "Bengaluru"}
      };
      
      var two = new Person()
      {
              Name = "One",
              Age = 1,
              Salary = 231.34m,
              IsMale = true,
              FavoriteColors = new HashSet<string>(){"blue", "green", "red"},
              PreviousAddresses = new List<string>(){"Delhi", "Mumbai", "Bengaluru"}
      };
      
      Console.WriteLine("one.Equals(two)" + one.Equals(two));
      Console.WriteLine("one.Equals(two)" + one.GetHashCode().Equals(two.GetHashCode()));
    }

    public static void SaveObject(IDatabase db)
    {
    }
  }
}