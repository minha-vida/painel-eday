using System;
using System.IO;
using System.Web;
using EdeaDay.GestaoDeIdeias.Repository;
using System.Drawing;

namespace EdeaDay.GestaoDeIdeias.Site
{
    /// <summary>
    /// Summary description for Image
    /// </summary>
    public class Image : IHttpHandler
    {
        private readonly Eday _context = new Eday();

        private IIdeiaRepository _ideiaRepository;

        public void ProcessRequest(HttpContext context)
        {
            var tamanhoMaximoImagem = new Size(194, 188);

            context.Response.ContentType = "image/jpg";

            ConfigurarRepositories();

            int ideiaId;
            int.TryParse(context.Request["iid"], out ideiaId);

            if (ideiaId == 0)
            {
                context.Response.StatusCode = 404;
            }

            var imagemDoBanco = _ideiaRepository.ObterImagemPorIdeiaId(ideiaId);

            var imagem = ConverterByteParaImagem(imagemDoBanco);

            if (!imagem.Size.Equals(tamanhoMaximoImagem))
            {
                imagemDoBanco = RedimensionarImagemParaTamanhoMaximo(imagem, tamanhoMaximoImagem);
            }

            context.Response.BinaryWrite(imagemDoBanco);
        }

        private byte[] RedimensionarImagemParaTamanhoMaximo(System.Drawing.Image imagem, Size tamanhoMaximoImagem)
        {
            var imagemRedimensionada = imagem.GetThumbnailImage(tamanhoMaximoImagem.Width, tamanhoMaximoImagem.Height, null, IntPtr.Zero);

            var imagemConvertida = ConverterImagemParaByte(imagemRedimensionada);

            return imagemConvertida;
        }

        private System.Drawing.Image ConverterByteParaImagem(byte[] imagemByte)
        {
            var ms = new MemoryStream(imagemByte);
            var imagem = System.Drawing.Image.FromStream(ms);

            return imagem;
        }

        public byte[] ConverterImagemParaByte(System.Drawing.Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }

        private void ConfigurarRepositories()
        {
            _ideiaRepository = new IdeiaRepository(_context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}