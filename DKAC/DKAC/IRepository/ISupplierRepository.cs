using DKAC.Models.EntityModel;
using DKAC.Models.InfoModel;
using DKAC.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DKAC.IRepository
{
    public interface ISupplierRepository
    {
        int Add(SupplierInfo model);
        int Update(SupplierInfo model);
        SupplierRequestModel GetAllSupplier(string KeySearch, int page, int pageSize);
        int Delete(Supplier sup);
        Supplier GetById(int? id);
        Supplier GetByCodeId(string Code, int id);

        List<Supplier> GetAll();
    }
}