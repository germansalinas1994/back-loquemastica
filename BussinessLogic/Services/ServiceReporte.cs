using System;
using System.Security.Cryptography.X509Certificates;
using BussinessLogic.DTO;
using DataAccess.IRepository;
using DataAccess.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BussinessLogic.DTO.Search;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace BussinessLogic.Services
{
    public class ServiceReporte
    {
        //Instancio el UnitOfWork que vamos a usar
        private readonly IUnitOfWork _unitOfWork;

        //Inyecto el UnitOfWork por el constructor, esto se hace para que se cree un nuevo contexto por cada vez que se llame a la clase
        public ServiceReporte(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<byte[]> CrearPDF(PedidoDTO pedido)

        {

            //descargo la imagen de lo que mastica tu mascota
            try
            {
                using var client = new HttpClient();

                // Descargar la imagen
                var imageStream = new MemoryStream();
                var response = await client.GetAsync("https://storage.googleapis.com/loquemasticatumascota/383349650_10234216208090669_4456284716581373004_n.jpg");
                await response.Content.CopyToAsync(imageStream);
                imageStream.Position = 0; // Rebobinar el stream para su uso en QuestPDF









                using var memoryStream = new MemoryStream();


                decimal totalPedido = 0;

                Document.Create(document =>
                {
                    document.Page(page =>
                    {

                        page.Margin(30);
                        page.Size(PageSizes.A4);
                        page.Header().ShowOnce().Row(row =>
                        {
                            row.ConstantItem(270).Height(80).Image(imageStream, ImageScaling.FitArea); // FitArea mantiene la proporción


                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Fecha de pedido " + pedido.FechaAlta?.ToString("dd/MM/yyyy")).FontSize(12);

                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Height(39).Border(1).BorderColor("#1f0f0f")
                                .AlignCenter().Text("Factura tipo B").FontSize(12);

                                col.Item().Height(39).Border(1)
                                .BorderColor("#1f0f0f").AlignCenter()
                                .Text("Pedido #" + pedido.Orden_MercadoPago);


                            });
                        });

                        page.Content().PaddingVertical(10).Column(col1 =>
                        {


                            col1.Item().Column(col2 =>
                                                {
                                                    col2.Item().AlignLeft().Text("Lo Que Mastica Tu Mascota").Bold().FontSize(14);
                                                    col2.Item().AlignLeft().Text(pedido.PublicacionPedido[0].Publicacion.IdSucursalNavigation.Nombre).FontSize(9);
                                                    col2.Item().AlignLeft().Text("221 6449147").FontSize(9);
                                                    col2.Item().AlignLeft().Text("consultas@loquemasticatumascota.com.ar").FontSize(9);
                                                });



                            col1.Item().Column(col2 =>
                            {
                                col2.Item().Text("Datos del cliente").Bold();

                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Nombre: ").SemiBold().FontSize(10);
                                    txt.Span(pedido.Usuario.Nombre).FontSize(10);
                                });

                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Apellido: ").SemiBold().FontSize(10);
                                    txt.Span(pedido.Usuario.Apellido).FontSize(10);
                                });
                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Email: ").SemiBold().FontSize(10);
                                    txt.Span(pedido.Usuario.Email).FontSize(10);
                                });
                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Dni: ").SemiBold().FontSize(10);
                                    txt.Span(pedido.Usuario.Dni.ToString()).FontSize(10);
                                });

                                if (pedido.Envio != null)
                                {
                                    col2.Item().Text(txt =>
                                    {
                                        txt.Span("Direccion: ").SemiBold().FontSize(10);
                                        txt.Span(pedido.Envio.Domicilio.DescripcionCompleta).FontSize(10);
                                    });
                                }

                            });

                            col1.Item().LineHorizontal(0.5f);

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell()
                                    .Padding(2).Text("Producto");

                                    header.Cell()
                                   .Padding(2).Text("Cantidad");

                                    header.Cell()
                                  .Padding(2).Text("Precio Unit");

                                    header.Cell()
                                   .Padding(2).Text("Total");
                                });

                                foreach (PublicacionPedidoDTO item in pedido.PublicacionPedido)
                                {
                                    var cantidad = item.Cantidad;
                                    var precio = item.Precio;
                                    var total = item.Cantidad * item.Precio;
                                    totalPedido += total;

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text(item.Publicacion.IdProductoNavigation.Nombre).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text("$" + precio).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).AlignRight().Text("$" + total).FontSize(10);
                                }

                            });

                            col1.Item().Background(Colors.Grey.Lighten3).AlignRight().Text("Total: $" + totalPedido).FontSize(12);


                            col1.Spacing(10);
                        });


                        page.Footer()
                        .AlignRight()
                        .Text(txt =>
                        {
                            txt.Span("Pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                    });
                }).GeneratePdf(memoryStream);


                memoryStream.Position = 0;


                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

         public async Task<byte[]> ReportePedidos(List<PedidoDTO> pedidos)

        {

            //descargo la imagen de lo que mastica tu mascota
            try
            {
                using var client = new HttpClient();

                // Descargar la imagen
                var imageStream = new MemoryStream();
                var response = await client.GetAsync("https://storage.googleapis.com/loquemasticatumascota/383349650_10234216208090669_4456284716581373004_n.jpg");
                await response.Content.CopyToAsync(imageStream);
                imageStream.Position = 0; // Rebobinar el stream para su uso en QuestPDF









                using var memoryStream = new MemoryStream();


                // decimal totalPedido = 0;

                Document.Create(document =>
                {
                    document.Page(page =>
                    {

                        page.Margin(30);
                        page.Size(PageSizes.A4);
                        page.Header().ShowOnce().Row(row =>
                        {
                            row.ConstantItem(270).Height(80).Image(imageStream, ImageScaling.FitArea); // FitArea mantiene la proporción


                         

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Height(39).Border(1).BorderColor("#1f0f0f")
                                .AlignCenter().Text("Reporte Mensual").FontSize(12);

                                col.Item().Height(39).Border(1)
                                .BorderColor("#1f0f0f").AlignCenter()
                                .Text("");  // aca deberia mostar el mes y año


                            });
                        });

                        page.Content().PaddingVertical(10).Column(col1 =>
                        {

                            col1.Item().Column(col2 =>
                                                {
                                                    col2.Item().AlignLeft().Text("Lo Que Mastica Tu Mascota").Bold().FontSize(14);
                                                    col2.Item().AlignLeft().Text(pedidos[0].PublicacionPedido[0].Publicacion.IdSucursalNavigation.Direccion).FontSize(9);
                                                });



                            col1.Item().Column(col2 =>
                            { col2.Item().AlignCenter().Text("Reporte Mensual de Ventas").Bold().FontSize(18); });

                            col1.Item().LineHorizontal(0.5f);

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();

                                });

                                tabla.Header(header =>
                                {
                                    header.Cell()
                                    .Padding(2).Text("Producto");

                                    header.Cell()
                                   .Padding(2).Text("Cantidad");

                                    header.Cell()
                                  .Padding(2).Text("Precio Unit");

                                    header.Cell()
                                   .Padding(2).Text("Total");
                                });

                                foreach (PedidoDTO item in pedidos)
                                {
                                    totalPedido += total;

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text(item.Publicacion.IdProductoNavigation.Nombre).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text("$" + precio).FontSize(10);

                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).AlignRight().Text("$" + total).FontSize(10);
                                }

                            });

                            col1.Item().Background(Colors.Grey.Lighten3).AlignRight().Text("Total: $" + totalPedido).FontSize(12);


                            col1.Spacing(10);
                        });


                        page.Footer()
                        .AlignRight()
                        .Text(txt =>
                        {
                            txt.Span("Pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                    });
                }).GeneratePdf(memoryStream);


                memoryStream.Position = 0;


                return memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}


