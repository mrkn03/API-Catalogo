﻿namespace APICatalogo.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        void Commit();
        void Dispose();

    }
}
