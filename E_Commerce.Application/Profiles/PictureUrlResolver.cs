using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Profiles
{
    internal class PictureUrlResolver : IValueResolver<Product, ProductDTo, string>
    {
        private readonly UrlSettings _urlSettings;

        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
           _urlSettings= options.Value;
        }
        public string Resolve(Product source, ProductDTo destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";

        }
    }
    public class UrlSettings
    {
        public string BaseUrl { get; set; }
    }
}
