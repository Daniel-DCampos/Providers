using Microsoft.AspNetCore.Mvc;
using Prov.Business.Services_Interfaces;

namespace Prov.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador; 
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            notificacoes.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Mensagem));
            return View();
        }
    }
}
