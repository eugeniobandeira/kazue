﻿namespace kazue
{
    public class ClienteEntidade
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Servico { get; set; } = string.Empty;
        public string? BarbeiroPreferido { get; set; }
        public DateTime RegistradoEm { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}