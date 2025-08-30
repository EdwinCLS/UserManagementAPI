
using System.Collections.Generic;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
  public static class UserStore
  {
    public static List<User> Users { get; } = new List<User>();
        private static int _nextId = 1;

        public static int GetNextId() => _nextId++;

  }
}