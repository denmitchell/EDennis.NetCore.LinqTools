using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Tests {
    public class StudentRepo {

        public static List<Student> Students { get; set; }

        static StudentRepo() {
            Students = new List<Student>() {
              new Student {
                ID =  1,
                LastName =  "Alexander",
                FirstMidName =  "Carson",
                EnrollmentDate = DateTime.Parse("2010-09-01T00:00:00")
              },
              new Student {
                ID =  2,
                LastName =  "Alonso",
                FirstMidName =  "Meredith",
                EnrollmentDate = DateTime.Parse("2012-09-01T00:00:00")
              },
              new Student {
                ID =  3,
                LastName =  "Anand",
                FirstMidName =  "Arturo",
                EnrollmentDate =  DateTime.Parse("2013-09-01T00:00:00")
              },
              new Student {
                ID =  4,
                LastName =  "Barzdukas",
                FirstMidName =  "Gytis",
                EnrollmentDate =  DateTime.Parse("2012-09-01T00:00:00")
              },
              new Student {
                ID =  6,
                LastName =  "Justice",
                FirstMidName =  "Peggy",
                EnrollmentDate =  DateTime.Parse("2011-09-01T00:00:00")
              },
              new Student {
                ID =  5,
                LastName =  "Li",
                FirstMidName =  "Yan",
                EnrollmentDate =  DateTime.Parse("2012-09-01T00:00:00")
              },
              new Student {
                ID =  7,
                LastName =  "Norman",
                FirstMidName =  "Laura",
                EnrollmentDate =  DateTime.Parse("2013-09-01T00:00:00")
              },
              new Student {
                ID =  8,
                LastName =  "Olivetto",
                FirstMidName =  "Nino",
                EnrollmentDate =  DateTime.Parse("2005-09-01T00:00:00")
              }
            };
        }
    }
}
