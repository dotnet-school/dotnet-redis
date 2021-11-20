using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
  public class Person
  {
    public string Name { get; set; } = String.Empty;
    public int Age { get; set; } 
    public decimal Salary { get; set; }
    public bool IsMale { get; set; }
    public IList<string> PreviousAddresses { get; set; } = new List<string>();
    public ISet<string> FavoriteColors { get; set; } = new HashSet<string>();

    public override bool Equals(object? that)
    {
      var otherPerson = that as Person;
      if (otherPerson != null)
      {
        return Name.Equals(otherPerson.Name) &&
               Age.Equals(otherPerson.Age) &&
               IsMale.Equals(otherPerson.IsMale) &&
               PreviousAddressesAsString().Equals(otherPerson.PreviousAddressesAsString()) &&
               FavColorsAsString().Equals(otherPerson.FavColorsAsString());
      }

      return false;
    }

    public override int GetHashCode()
    {
      return Name.GetHashCode() 
             + Age.GetHashCode() 
             + IsMale.GetHashCode() 
             + PreviousAddressesAsString().GetHashCode() 
             + FavColorsAsString().GetHashCode();
    }

    private string PreviousAddressesAsString()
    {
      return string.Join(',', PreviousAddresses);
    }
    private string FavColorsAsString()
    {
      var sorted = FavoriteColors.ToArray();
      Array.Sort(sorted);
      return string.Join(',', sorted);
    }
  }
}