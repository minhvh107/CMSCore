using CMSCore.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSCore.Application.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        void Add(FunctionViewModel function);

        Task<List<FunctionViewModel>> GetAllAsync(string filter);

        IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId);

        FunctionViewModel GetById(string id);

        void Update(FunctionViewModel function);

        void Delete(string id);

        void Save();

        bool CheckExistedId(string id);

        void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items);

        void ReOrder(string sourceId, string targetId);
    }
}