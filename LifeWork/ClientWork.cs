using LifeWork.Model;
using MainLibrary.Interfaces;
using System;
using System.Threading.Tasks;

namespace LifeWork
{
    public class ClientWork : IClientCode
    {
        public Task<byte[]> Entrypoint(byte[] args)
        {
            int fieldSize = 5;
            bool[,] input = new bool[fieldSize, fieldSize];
            for (int i = 0; i < fieldSize; i++)
                for (int j = 0; j < fieldSize; j++)
                    input[i, j] = args[fieldSize * i + j] > 0;

            Board board = new(fieldSize, fieldSize, input);
            board.Turn(10);

            byte[] answer = new byte[fieldSize * fieldSize];
            for (int i = 0; i < fieldSize; i++)
                for (int j = 0; j < fieldSize; j++)
                    answer[fieldSize*i+j]= (byte)(board.Cells[i,j].IsAlive?1:0);
            return Task.FromResult(answer);
        }
    }
}
