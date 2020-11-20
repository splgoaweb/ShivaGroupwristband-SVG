using Nop.Core.Caching;
using Nop.Core.Domain.SVG;
using Nop.Data;
using System.Linq;

namespace Nop.Services.SVG
{
    public partial class SVGContentService : ISVGContentService
    {
        private readonly CachingSettings _cachingSettings;
        private readonly IRepository<SVGContent> _SVGContentRepository;

        #region Ctor

        public SVGContentService(CachingSettings cachingSettings,
            IRepository<SVGContent> SVGContentRepository)
        {
            _cachingSettings = cachingSettings;
            _SVGContentRepository = SVGContentRepository;
        }

        #endregion
        public virtual SVGContent GetContentByProductId(int productId)
        {
            if (productId < 0)
                return null;

            return _SVGContentRepository.Table.FirstOrDefault(o => o.pID == productId);
        }
    }
}

