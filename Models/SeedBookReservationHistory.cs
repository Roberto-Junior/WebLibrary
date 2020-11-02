using BiblioTechA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BiblioTechA.Models
{
    public class SeedBookReservationHistory
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.BookReservationHistory.Any())
                {
                    return;
                }

                context.BookReservationHistory.AddRange(
                    new BookReservationHistory
                    {
                        Title = "À procura da felicidade",
                        Author = "Chris Gardner",
                        Description = "O livro conta a história de um vendedor, que depois virou corretor de investimentos, que teve que passar pelas maiores dificuldades possíveis para conseguir ter sucesso em seu emprego. Passou fome, frio e vergonha, tudo isso tendo que sustentar o seu filho pequeno.",
                        WhoReserved = "usuarioTeste1@test.com",
                        WhoReleased = "adminTeste1@test.com",
                        WhoReceivedReturn = "adminTeste2@test.com",
                        ReservationDateRelease = new DateTime(2010, 1, 2, 12, 30, 10),
                        ReservationDateReturn = new DateTime(2010, 2, 3, 12, 50, 30)
                    },

                    new BookReservationHistory
                    {
                        Title = "Eu sou a lenda",
                        Author = "Richard Matheson",
                        Description = "Uma impiedosa praga assola o mundo, transformando cada homem, mulher e criança do planeta em algo digno dos pesadelos mais sombrios. Nesse cenário pós-apocalíptico, tomado por criaturas da noite sedentas de sangue, Robert Neville pode ser o último homem na Terra. Ele passa seus dias em busca de comida e suprimentos, lutando para manter-se vivo (e são).",
                        WhoReserved = "usuarioTeste2@test.com",
                        WhoReleased = "adminTeste3@test.com",
                        WhoReceivedReturn = "adminTeste4@test.com",
                        ReservationDateRelease = new DateTime(2011, 8, 2, 15, 17, 22),
                        ReservationDateReturn = new DateTime(2011, 9, 3, 3, 10, 2)
                    },

                    new BookReservationHistory
                    {
                        Title = "O hobbit",
                        Author = "J. R. R. Tolkien",
                        Description = "O Hobbit conta a história de Bilbo Bolseiro, um Hobbit pacato e satisfeito cuja vida vira de cabeça para baixo quando ele se junta ao mago Gandalf e a treze anões em sua jornada para reaver um tesouro roubado.",
                        WhoReserved = "usuarioTeste3@test.com",
                        WhoReleased = "adminTeste5@test.com",
                        WhoReceivedReturn = "adminTeste@test.com",
                        ReservationDateRelease = new DateTime(2013, 1, 13, 18, 45, 23),
                        ReservationDateReturn = new DateTime(2013, 3, 22, 10, 22, 21)
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
