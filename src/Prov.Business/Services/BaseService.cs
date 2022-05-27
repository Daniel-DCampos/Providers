using FluentValidation;
using FluentValidation.Results;
using Prov.Business.Models;
using Prov.Business.Notificacoes;
using Prov.Business.Services_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotificador _notificator;

        public BaseService(INotificador notificador)
        {
            _notificator = notificador;
        }
        protected void Notificar(string errorMessage)
        {
            _notificator.Handle(new Notificacao(errorMessage));
        }
        protected void Notificar(ValidationResult validationResult)
        {
            validationResult.Errors.ForEach(e => Notificar(e.ErrorMessage));
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entidade
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}
