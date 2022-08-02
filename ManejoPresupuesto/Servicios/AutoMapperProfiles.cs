using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CuentaModel, CuentaCreacionViewModel>();
            CreateMap<TransaccionActualizacionViewModel, TransaccionModel>().ReverseMap();
            // ReverseMap() permite realizar el mapeo desde {1} a {2} y de {2} a {1}
        }
    }
}
