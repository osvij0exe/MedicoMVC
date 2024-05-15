using MedicoMVC.DataAcces;
using MedicoMVC.Models.Request;
using MedicoMVC.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medico.NUnitTests
{
    [TestFixture]
    public class Medico
    {
        private MedicoDTORequest MedicoRequestTest1;
        private MedicoDTORequest MedicoRequestTest2;
        private MedicoDTOResponse MedicoRespons1;
        private MedicoDTOResponse MedicoRespons2;
        private readonly SettingStrings _Conections;
        
        [SetUp]
        public void SetUp()
        {
           
        }

    }
}
