using BiblioTechA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BiblioTechA.Models
{
    public class SeedBook
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.Book.Any())
                {
                    return;
                }

                context.Book.AddRange(
                    new Book
                    {
                        Title = "À procura da felicidade",
                        Author = "Chris Gardner",
                        Genre = "Biografias e Casos Verdadeiros",
                        PageNumbers = 320,
                        Description = "O livro conta a história de um vendedor, que depois virou corretor de investimentos, que teve que passar pelas maiores dificuldades possíveis para conseguir ter sucesso em seu emprego. Passou fome, frio e vergonha, tudo isso tendo que sustentar o seu filho pequeno.",
                        Reserved = "Free",
                        WhoReserved = "None",
                        WhoReleased = "None"
                    },

                    new Book
                    {
                        Title = "A escola dos deuses",
                        Author = "Elio D'Anna",
                        Genre = "Empreendimento",
                        PageNumbers = 408,
                        Description = "Narra a história do renascimento de um ser humano comum, que encontra um ser extraordinário - o Dreamer - e ao lado dele aprende a sair da mediocridade de uma vida infeliz, mecânica e repetitiva, de pensamentos obsoletos e hábitos arraigados, e ir ao encontro do seu sonho, um retorno a um estado de integridade, por intermédio de uma revolução individual.",
                        Reserved = "Free",
                        WhoReserved = "None",
                        WhoReleased = "None"
                    },

                    new Book
                    {
                        Title = "O poder do hábito",
                        Author = "Charles Duhigg",
                        Genre = "Comportamento Organizacional",
                        PageNumbers = 408,
                        Description = "O livro O poder do hábito escrito pelo repórter do New York Times Charles Duhigg, que há duas décadas pesquisou como os hábitos funcionam – e, mais importante, como podem ser transformados, elabora um argumento animador: A chave para mudar o que não funciona em sua vida é entender como os hábitos funcionam.",
                        Reserved = "Free",
                        WhoReserved = "None",
                        WhoReleased = "None"
                    },

                     new Book
                     {
                         Title = "Essencialismo",
                         Author = "Greg Mckeown",
                         Genre = "Administração, Negócios e Economia",
                         PageNumbers = 272,
                         Description = "Se você se sente sobrecarregado e ao mesmo tempo subutilizado, ocupado, mas pouco produtivo, e se o seu tempo parece servir apenas aos interesses dos outros, você precisa conhecer o essencialismo.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "Rápido e devagar",
                         Author = "Daniel Kahneman",
                         Genre = "Decisões e Resolução de Problemas",
                         PageNumbers = 608,
                         Description = "Em Rápido e devagar: duas formas de pensar, Daniel Kahneman nos leva a uma viagem pela mente humana e explica as duas formas de pensar: uma é rápida, intuitiva e emocional; a outra, mais lenta, deliberativa e lógica.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "Ponto de inflexão",
                         Author = "Flávio Augusto Da Silva",
                         Genre = "Administração e Economia",
                         PageNumbers = 208,
                         Description = "Ponto de Inflexão traz um relato objetivo e sincero dos pontos na vida do Flávio em que ele precisou tomar decisões importantes que decidiram cenários importantes na sua vida.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "Meditações",
                         Author = "Marco Aurélio",
                         Genre = "Política e Ciências Sociais",
                         PageNumbers = 160,
                         Description = "Também conhecidas como Meditações a mim mesmo, reúnem aforismos que orientaram o governante pela perspectiva do estoicismo – o controle das emoções para que se evitem os erros de julgamento.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "O conto da aia",
                         Author = "Margaret Atwood",
                         Genre = "Ficção Científica Distópico",
                         PageNumbers = 368,
                         Description = "O romance distópico O conto da aia, de Margaret Atwood, se passa num futuro muito próximo e tem como cenário uma república onde não existem mais jornais, revistas, livros nem filmes. As universidades foram extintas. Também já não há advogados, porque ninguém tem direito a defesa. Os cidadãos considerados criminosos são fuzilados e pendurados mortos no Muro, em praça pública, para servir de exemplo enquanto seus corpos apodrecem à vista de todos.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "A garota do lago",
                         Author = "Charlie Donlea",
                         Genre = "Literatura e Ficção",
                         PageNumbers = 296,
                         Description = "Summit Lake, uma pequena cidade entre montanhas, é esse tipo de lugar, bucólico e com encantadoras casas dispostas à beira de um longo trecho de água intocada.Duas semanas atrás, a estudante de direito Becca Eckersley foi brutalmente assassinada em uma dessas casas. Filha de um poderoso advogado, Becca estava no auge de sua vida. Atraída instintivamente pela notícia, a repórter Kelsey Castle vai até a cidade para investigar o caso.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                     new Book
                     {
                         Title = "Harry Potter e a pedra filosofal",
                         Author = "J.K. Rowling",
                         Genre = "Ficção Científica Fantasia",
                         PageNumbers = 208,
                         Description = "O livro conta a história de Harry Potter, um órfão criado pelos tios que descobre, em seu décimo primeiro aniversário, que é um bruxo.",
                         Reserved = "Free",
                         WhoReserved = "None",
                         WhoReleased = "None"
                     },

                      new Book
                      {
                          Title = "Eu sou a lenda",
                          Author = "Richard Matheson",
                          Genre = "Ficção científica",
                          PageNumbers = 213,
                          Description = "Uma impiedosa praga assola o mundo, transformando cada homem, mulher e criança do planeta em algo digno dos pesadelos mais sombrios. Nesse cenário pós-apocalíptico, tomado por criaturas da noite sedentas de sangue, Robert Neville pode ser o último homem na Terra. Ele passa seus dias em busca de comida e suprimentos, lutando para manter-se vivo (e são).",
                          Reserved = "Free",
                          WhoReserved = "None",
                          WhoReleased = "None"
                      },

                    new Book
                    {
                        Title = "O hobbit",
                        Author = "J. R. R. Tolkien",
                        Genre = "Ficção Científica Fantasia",
                        PageNumbers = 328,
                        Description = "O Hobbit conta a história de Bilbo Bolseiro, um Hobbit pacato e satisfeito cuja vida vira de cabeça para baixo quando ele se junta ao mago Gandalf e a treze anões em sua jornada para reaver um tesouro roubado.",
                        Reserved = "Free",
                        WhoReserved = "None",
                        WhoReleased = "None"
                    }
                ) ;
                context.SaveChanges();
            }
        }
    }
}
