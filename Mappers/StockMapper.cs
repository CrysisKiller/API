using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stockModel){
            return new StockDto{
                Id=stockModel.Id,
                Symbol=stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv =   stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,  
                MyProperty = stockModel.MyProperty.Select(c=>c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToRequestDto(this CreateStockRequestDto dto){
            return new Stock{
                Symbol=dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDiv =   dto.LastDiv,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap  
            };
        }
    }
}