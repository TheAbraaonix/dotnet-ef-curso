using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogoxUnitTest.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            //Arrange
            var prodId = 14;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "produto atualizado",
                Descricao = "Descrição do produto atualizado",
                Preco = 10.99m,
                ImagemUrl = "imagemteste.jpg",
                CategoriaId = 2
            };

            //Act
            var result = await _controller.Put(prodId, updatedProdutoDto) as ActionResult<ProdutoDTO>;

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PutProduto_Return_BadRequest()
        {
            //Arrange
            var prodId = 1000;

            var updatedProdutoDto = new ProdutoDTO
            {
                ProdutoId = 14,
                Nome = "produto atualizado",
                Descricao = "Descrição do produto atualizado",
                Preco = 10.99m,
                ImagemUrl = "imagemteste.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.Put(prodId, updatedProdutoDto);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
        }
    }
}
