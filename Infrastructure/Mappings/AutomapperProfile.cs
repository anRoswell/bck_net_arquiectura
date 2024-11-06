using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Proveedores, ProveedorDto>().ReverseMap();
            CreateMap<PermisosXUsuario, PermisosXUsuarioDto>().ReverseMap();
            CreateMap<CertificadosEspeciale, CertificadosEspecialesDto>().ReverseMap();
            CreateMap<Noticia, NoticiaDto>().ReverseMap();
            CreateMap<Perfil, PerfilDto>().ReverseMap();
            CreateMap<RespuestasCertEspeciale, RespuestasCertEspecialeDto>().ReverseMap();
            CreateMap<Contrato, ContratoDto>().ReverseMap();
            CreateMap<ParametrosRequerimientos, ParametrosRequerimientosDto>().ReverseMap();
            CreateMap<Documento, DocumentoDto>().ReverseMap();
            CreateMap<UnidadNegocio, UnidadesNegocioDto>().ReverseMap();
            CreateMap<TipoMinuta, TipoMinutaDto>().ReverseMap();
            CreateMap<ProveedorDto, TerceroDto>().ReverseMap();
            CreateMap<Tercero, TerceroDto>().ReverseMap();
            CreateMap<RepresentantesLegalEmpresa, RepresentantesLegalEmpresaDto>().ReverseMap();
            CreateMap<RepresentantesLegalEmpresaDto, RepresentantesLegalEmpresa>().ReverseMap();
            CreateMap<Op360GuardarOrden, Op360GuardarOrdenDto>().ReverseMap();

            //clave: aaaaORDENESa65sd4f65sdf _01c
            CreateMap<Aire_Scr_OrdenConGeorreferencia, Aire_Scr_OrdenListaDto>().ReverseMap();            
        }
    }
}
