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

                context.Database.OpenConnection();
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Book ON;");

                context.Book.AddRange(
                    new Book
                    {
                        Id = 1,
                        Title = "À procura da felicidade",
                        PageNumbers = 320,
                        Description = "O livro conta a história de um vendedor, que depois virou corretor de investimentos, que teve que passar pelas maiores dificuldades possíveis para conseguir ter sucesso em seu emprego. Passou fome, frio e vergonha, tudo isso tendo que sustentar o seu filho pequeno.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                   new Book
                   {
                       Id = 2,
                       Title = "A escola dos deuses",
                       PageNumbers = 408,
                       Description = "Narra a história do renascimento de um ser humano comum, que encontra um ser extraordinário - o Dreamer - e ao lado dele aprende a sair da mediocridade de uma vida infeliz, mecânica e repetitiva, de pensamentos obsoletos e hábitos arraigados, e ir ao encontro do seu sonho, um retorno a um estado de integridade, por intermédio de uma revolução individual.",
                       Reserved = "Free",
                       ReservationDateRelease = DateTime.Now
                   },

                   new Book
                   {
                       Id = 3,
                       Title = "O poder do hábito",
                       PageNumbers = 408,
                       Description = "O livro O poder do hábito escrito pelo repórter do New York Times Charles Duhigg, que há duas décadas pesquisou como os hábitos funcionam – e, mais importante, como podem ser transformados, elabora um argumento animador: A chave para mudar o que não funciona em sua vida é entender como os hábitos funcionam.",
                       Reserved = "Free",
                       ReservationDateRelease = DateTime.Now
                   },

                    new Book
                    {
                        Id = 4,
                        Title = "Essencialismo",
                        PageNumbers = 272,
                        Description = "Se você se sente sobrecarregado e ao mesmo tempo subutilizado, ocupado, mas pouco produtivo, e se o seu tempo parece servir apenas aos interesses dos outros, você precisa conhecer o essencialismo.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 5,
                        Title = "Rápido e devagar",
                        PageNumbers = 608,
                        Description = "Em Rápido e devagar: duas formas de pensar, Daniel Kahneman nos leva a uma viagem pela mente humana e explica as duas formas de pensar: uma é rápida, intuitiva e emocional; a outra, mais lenta, deliberativa e lógica.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 6,
                        Title = "Ponto de inflexão",
                        PageNumbers = 208,
                        Description = "Ponto de Inflexão traz um relato objetivo e sincero dos pontos na vida do Flávio em que ele precisou tomar decisões importantes que decidiram cenários importantes na sua vida.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 7,
                        Title = "Meditações",
                        PageNumbers = 160,
                        Description = "Também conhecidas como Meditações a mim mesmo, reúnem aforismos que orientaram o governante pela perspectiva do estoicismo – o controle das emoções para que se evitem os erros de julgamento.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 8,
                        Title = "O conto da aia",
                        PageNumbers = 368,
                        Description = "O romance distópico O conto da aia, de Margaret Atwood, se passa num futuro muito próximo e tem como cenário uma república onde não existem mais jornais, revistas, livros nem filmes. As universidades foram extintas. Também já não há advogados, porque ninguém tem direito a defesa. Os cidadãos considerados criminosos são fuzilados e pendurados mortos no Muro, em praça pública, para servir de exemplo enquanto seus corpos apodrecem à vista de todos.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 9,
                        Title = "A garota do lago",
                        PageNumbers = 296,
                        Description = "Summit Lake, uma pequena cidade entre montanhas, é esse tipo de lugar, bucólico e com encantadoras casas dispostas à beira de um longo trecho de água intocada.Duas semanas atrás, a estudante de direito Becca Eckersley foi brutalmente assassinada em uma dessas casas. Filha de um poderoso advogado, Becca estava no auge de sua vida. Atraída instintivamente pela notícia, a repórter Kelsey Castle vai até a cidade para investigar o caso.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                    new Book
                    {
                        Id = 10,
                        Title = "Harry Potter e a pedra filosofal",
                        PageNumbers = 208,
                        Description = "O livro conta a história de Harry Potter, um órfão criado pelos tios que descobre, em seu décimo primeiro aniversário, que é um bruxo.",
                        Reserved = "Free",
                        ReservationDateRelease = DateTime.Now
                    },

                     new Book
                     {
                         Id = 11,
                         Title = "Eu sou a lenda",
                         PageNumbers = 213,
                         Description = "Uma impiedosa praga assola o mundo, transformando cada homem, mulher e criança do planeta em algo digno dos pesadelos mais sombrios. Nesse cenário pós-apocalíptico, tomado por criaturas da noite sedentas de sangue, Robert Neville pode ser o último homem na Terra. Ele passa seus dias em busca de comida e suprimentos, lutando para manter-se vivo (e são).",
                         Reserved = "Free",
                         ReservationDateRelease = DateTime.Now
                     },

                   new Book
                   {
                       Id = 12,
                       Title = "O hobbit",
                       PageNumbers = 328,
                       Description = "O Hobbit conta a história de Bilbo Bolseiro, um Hobbit pacato e satisfeito cuja vida vira de cabeça para baixo quando ele se junta ao mago Gandalf e a treze anões em sua jornada para reaver um tesouro roubado.",
                       Reserved = "Free",
                       ReservationDateRelease = DateTime.Now
                   }
               ) ;

                
                context.SaveChanges();
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT dbo.Book OFF");
                context.Database.CloseConnection();

                context.BookAuthor.AddRange(
                     new BookAuthor
                     {
                         Author = "Chris Gardner",
                         BookId = 1
                     },

                    new BookAuthor
                    {
                        Author = "Elio D'Anna",
                        BookId = 2
                    },

                    new BookAuthor
                    {
                        Author = "Charles Duhigg",
                        BookId = 3
                    },

                     new BookAuthor
                     {
                         Author = "Greg Mckeown",
                         BookId = 4
                     },

                     new BookAuthor
                     {
                         Author = "Daniel Kahneman",
                         BookId = 5
                     },

                     new BookAuthor
                     {
                         Author = "Flávio Augusto Da Silva",
                         BookId = 6
                     },

                     new BookAuthor
                     {
                         Author = "Marco Aurélio",
                         BookId = 7
                     },

                     new BookAuthor
                     {
                         Author = "Margaret Atwood",
                         BookId = 8
                     },

                     new BookAuthor
                     {
                         Author = "Charlie Donlea",
                         BookId = 9
                     },

                     new BookAuthor
                     {
                         Author = "J.K. Rowling",
                         BookId = 10
                     },

                      new BookAuthor
                      {
                          Author = "Richard Matheson",
                          BookId = 11
                      },

                    new BookAuthor
                    {
                        Author = "J. R. R. Tolkien",
                        BookId = 12
                    }
                );

                context.SaveChanges();

                context.BookGenre.AddRange(
                     new BookGenre
                     {
                         Genre = "Biografias e Casos Verdadeiros",
                         BookId = 1
                     },

                    new BookGenre
                    {
                        Genre = "Empreendimento",
                        BookId = 2
                    },

                    new BookGenre
                    {
                        Genre = "Comportamento Organizacional",
                        BookId = 3
                    },

                     new BookGenre
                     {
                         Genre = "Administração, Negócios e Economia",
                         BookId = 4
                     },

                     new BookGenre
                     {
                         Genre = "Decisões e Resolução de Problemas",
                         BookId = 5
                     },

                     new BookGenre
                     {
                         Genre = "Administração e Economia",
                         BookId = 6
                     },

                     new BookGenre
                     {
                         Genre = "Política e Ciências Sociais",
                         BookId = 7
                     },

                     new BookGenre
                     {
                         Genre = "Ficção Científica Distópico",
                         BookId = 8
                     },

                     new BookGenre
                     {
                         Genre = "Literatura e Ficção",
                         BookId = 9
                     },

                     new BookGenre
                     {
                         Genre = "Ficção Científica Fantasia",
                         BookId = 10
                     },

                      new BookGenre
                      {
                          Genre = "Ficção científica",
                          BookId = 11
                      },

                    new BookGenre
                    {
                        Genre = "Ficção Científica Fantasia",
                        BookId = 12
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
