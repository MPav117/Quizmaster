using System.Net;
using Quizmaster.Models;

namespace Quizmaster.Datatypes {
    public struct GetQuizReturnValue 
    {
        public Quiz quiz;
        public List<QuizQuestion> quizQuestions;
    }
}