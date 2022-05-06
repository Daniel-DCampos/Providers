namespace Fornecedores.Models
{
    public abstract class Entidade
    {
        protected Entidade()
        {
            Id = new Guid();
            DataCriacao = DateTime.Now;
        }
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public Guid? UsuarioCriacao { get; set; }
    }
}
