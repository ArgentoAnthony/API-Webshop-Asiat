using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_DAL.Models;
using Webshop_DAL.Services;

namespace Webshop_DAL.Interfaces
{
    public interface ICommentaireEvaluationService : IBaseService<Commentaires>
    {
        bool RatingProduct(Evaluation rating, int? id);
        string LeaveComment(Commentaires commentaire, int? id);
        bool UpdateComment(Commentaires commentaire, int? id);
        bool DeleteComment(int commentaire, int? id);
        IEnumerable<Commentaires> GetCommentsByProduct(int idProduct);
    }
}
