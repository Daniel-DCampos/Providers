using AutoMapper;
using Fornecedores.Models;
using Prov.App.DTO;

namespace Prov.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Endereco, EnderecoDTO>().ReverseMap();
        }
    }
}
