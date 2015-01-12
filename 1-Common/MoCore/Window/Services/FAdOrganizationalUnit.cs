using System;
using System.Collections.Generic;
using System.Text;

namespace MO.Core.Window.Services {

   public class FAdOrganizationalUnit : FAdObject{

      public const string CLASS_NAME = "organizationalUnit";

      private FAdGroups _groups;

      private FAdUsers _users;

      public FAdOrganizationalUnit() { 
      }

      public FAdOrganizationalUnit(FAdConnection connection)
         : base(connection) {
      }

      public FAdGroups Groups {
         get{
            if(_groups == null){
               _groups = GetGroups();
            }
            return _groups;
         }
      }

      public FAdUsers Users {
         get {
            if (_users == null) {
               _users = GetUsers();
            }
            return _users;
         }
      }

      public FAdGroups GetGroups() {
         _groups = new FAdGroups();
         return _groups;
      }

      public FAdUsers GetUsers() {
         _users = new FAdUsers();
         return _users;
      }

   }

}
