using ERacuni.Server.Context;
using ERacuni.Shared.Convertors;
using ERacuni.Shared.Models;
using ERacuniProtoNameSpace;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ERacuni.Server.Services
{
    public class BillService : BillProtoServis.BillProtoServisBase
    {
        private readonly ILogger<BillService> _logger;
        private readonly BillDBContext _context;
        private readonly ConvertGRPC _convert;

        public BillService(ILogger<BillService> log, BillDBContext context, ConvertGRPC convert)
        {
            _logger = log;
            _context = context;
            _convert = convert;
        }

        public override async Task<AddBillResponse> AddBill(AddBillRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Usao u dodavanje racuna");
            await _context.Bills.ToListAsync();
            Bill bill = _convert.Convert(request.BillForRequest);
            try
            {
                _logger.LogInformation("usao u try");
                if (_context.Bills.Find(bill.barcode) is not null || bill.barcode==" ")
                {
                    _logger.LogInformation("Nije Dodao Racun sa Barcodom" + bill.barcode + " jer postoji u bazi!");
                    return new AddBillResponse { Success = false, Message = "Ovaj Barcode već postoji! " };
                    
                }
                else
                {
                    await _context.Bills.AddAsync(bill);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Dodao Racun sa Barcodom" + bill.barcode);
                    return new AddBillResponse { Success = true, Message = "Uspelo je upisivanje u bazu", BillForResponse = request.BillForRequest };
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation("usao u catch");
                _logger.LogError(ex.Message);
                return new AddBillResponse { Success = false, Message = "Ovaj Barcode već postoji! " };
            }
        }

        public override async Task GetAllBills(EmptyMsg request, IServerStreamWriter<BillMsg> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("[Usli smo u GetAllBills]");

            var bills = _context.Bills.ToList();

            try
            {
                foreach (var item in bills)
                {
                    _logger.LogInformation("[Poceo je stream]");
                    await responseStream.WriteAsync(_convert.Convert(item));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("[zzzzzzzavrsio sam strimovanje]");
        }

        public override async Task<DeleteResponse> DeleteBill(BillMsg request, ServerCallContext context)
        {
            await _context.Bills.ToListAsync();
            if (_context.Bills.Find(request.Barcode) is not null)
            {
                _logger.LogInformation(request.Barcode);
                _context.Bills.Remove(_context.Bills.Find(request.Barcode));
                try
                {
                    await _context.SaveChangesAsync();
                    return new DeleteResponse { Status = true };
                }
                catch (Exception ex)
                {
                    return new DeleteResponse { Status = false };
                }
            }
            else
            {
                return new DeleteResponse { Status = false };
            }
        }

        public override async Task<ChangeBillResponse> ChangeBill(BillMsg request, ServerCallContext context)
        {
            var comparativeBill = _context.Bills.Find(request.Barcode);
            _logger.LogInformation($"Nadjen je racun sa tim barcodom u bazi i barcode je {comparativeBill.barcode}");

            if (comparativeBill is not null)
            {
                _logger.LogInformation("usao sam u if comparativeBill is not null");
                comparativeBill.titleBill = _convert.Convert(request).titleBill;
                comparativeBill.customerAdress = _convert.Convert(request).customerAdress;
                comparativeBill.shippingFee = _convert.Convert(request).shippingFee;
                comparativeBill.ransom = _convert.Convert(request).ransom;
                comparativeBill.postingDate = _convert.Convert(request).postingDate;
                comparativeBill.receiptDate = _convert.Convert(request).receiptDate;
                comparativeBill.wayOfSelling = _convert.Convert(request).wayOfSelling;
                _logger.LogInformation("ovaj Bill sto sam poslao sam smestio u comparativeBill");
                try
                {
                    _logger.LogInformation("Usao sam u try da sacuvam to u bazu");
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("sacuvao sam u bazu promene");
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).barcode);
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).titleBill);
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).shippingFee.ToString());
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).postingDate.ToString());
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).receiptDate.ToString());
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).ransom.ToString());
                    _logger.LogInformation(_context.Bills.Find(comparativeBill.barcode).wayOfSelling);
                    return new ChangeBillResponse { ChangedBill = _convert.Convert(comparativeBill), Success = true };
                }
                catch (Exception ex)
                {
                    _logger.LogError("uhvatio sam gresku");
                    return new ChangeBillResponse { Success = false, Message = $"upisivanje u bazu nije uspelo + {ex.Message}" };
                }
            }
            else
            {
                _logger.LogInformation("comparativeBill je null");
                return new ChangeBillResponse { Success = false, Message = "nije uspelo upisivanje u bazu" };
            }
        }

        public override async Task<ProductAndBoolMSG> AddProductMSG(ProductMSG request, ServerCallContext context)
        {
            if (_context.Products.Find(request.IDMSG) == null)
            {
                _context.Products.Add(_convert.Convert(request));
                await _context.SaveChangesAsync();
                _logger.LogInformation("Uspesno dodao novi artikal!");
                return new ProductAndBoolMSG { Success = true, ProdMSG = _convert.Convert(_context.Products.Find(request.IDMSG)) };
            }
            else
            {
                _context.Products.Find(request.IDMSG).contentProduct = request.ContentProductMSG;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Dodao kolicinu vec postojecem artiklu!");
                return new ProductAndBoolMSG { Success = false, ProdMSG = _convert.Convert(_context.Products.Find(request.IDMSG)) };
            }
        }

        public override async Task GetAllProduct(EmptyMsg request, IServerStreamWriter<ProductMSG> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("U GetAllProduct");
            try
            {
                foreach (var a in _context.Products.ToList())
                {
                    await responseStream.WriteAsync(_convert.Convert(a));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Greska!", ex);
            }
        }

        public override async Task<DeleteResponse> DeleteProduct(ProductMSG request, ServerCallContext context)
        {
            try
            {
                if (_context.Products.Find(request.IDMSG) is not null)
                {
                    _context.Products.Remove(_context.Products.Find(_convert.Convert(request).ID));
                    _logger.LogInformation("Product is deleted");
                    await _context.SaveChangesAsync();
                    return new DeleteResponse { Status = true };
                }
                else
                {
                    _logger.LogInformation("Product not deleted");
                    return new DeleteResponse { Status = false };
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return new DeleteResponse { Status = false };
            }
        }

        public override async Task ListForReport(ListForReportRequest request, IServerStreamWriter<BillMsg> responseStream, ServerCallContext context)
        {

            DateTime start = _convert.Convert(request).start;
            DateTime end = _convert.Convert(request).end;
            string wayofselling = _convert.Convert(request).wayOfSelling;
            string typeofdate = _convert.Convert(request).TypeOfDate;
            DateTime defaultDateTime = new DateTime(0001, 1, 1, 0, 0, 0);
            _logger.LogInformation(start.ToString());
            _logger.LogInformation(end.ToString());
            _logger.LogInformation(wayofselling);
            _logger.LogInformation(typeofdate);

            switch (typeofdate)
            {
                case "Poslati":
                    {
                        if (DateTime.Compare(start, defaultDateTime) == 0)
                        {
                            //nije selektovao opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {

                                _logger.LogInformation("nije selektovao opseg niti nacin slanja a prikazati samo poslate");
                                var bills = await _context.Bills.Where(b => DateTime.Compare(b.receiptDate, defaultDateTime) == 0).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                            else
                            {

                                _logger.LogInformation("nije selektovao opseg, selektovao je nacin slanja  a prikazati samo poslate");
                                var bills = await _context.Bills.Where(b => DateTime.Compare(b.receiptDate, defaultDateTime) == 0  && b.wayOfSelling.ToLower() == wayofselling.ToLower()).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                        }
                        else
                        {
                            //selektovao je opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {
                                _logger.LogInformation("selektovao je opseg, nije nacin slanja a prikazati samo poslate");
                                var bills = await _context.Bills.Where(b => DateTime.Compare(b.receiptDate, defaultDateTime) == 0 && b.postingDate >= start && b.receiptDate <= end).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                            else
                            {
                                _logger.LogInformation("selektovao opseg i nacin slanja a prikazati samo poslate");
                                var bills = await _context.Bills.Where(b => DateTime.Compare(b.receiptDate, defaultDateTime) == 0 && b.postingDate >= start && b.receiptDate <= end && b.wayOfSelling.ToLower() == wayofselling.ToLower()).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }

                            }
                        }
                    }
                    break;
                case "Primljeni":
                    {
                        if (DateTime.Compare(start, defaultDateTime) == 0)
                        {
                            //nije selektovao opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {

                                _logger.LogInformation("nije selektovao opseg niti nacin slanja a prikazati samo primljene");
                                var bills = await _context.Bills.Where(b=> !(DateTime.Compare(b.receiptDate, defaultDateTime)==0)).ToListAsync(); 
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }

                            }
                            else
                            {

                                _logger.LogInformation("nije selektovao opseg, selektovao je nacin slanja a prikazati samo primljene");
                                _logger.LogInformation("nije selektovao opseg niti nacin slanja a prikazati samo primljene");
                                var bills = await _context.Bills.Where(b => !(DateTime.Compare(b.receiptDate, defaultDateTime) == 0) && b.wayOfSelling.ToLower()==wayofselling.ToLower()).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                        }
                        else
                        {
                            //selektovao je opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {
                                _logger.LogInformation("selektovao je opseg, nije nacin slanja a prikazati samo primljene");
                                var bills = await _context.Bills.Where(b => !(DateTime.Compare(b.receiptDate, defaultDateTime) == 0) && b.postingDate >= start && b.receiptDate <= end).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                            else
                            {
                                _logger.LogInformation("selektovao opseg i nacin slanja a prikazati samo primljene");
                                var bills = await _context.Bills.Where(b => !(DateTime.Compare(b.receiptDate, defaultDateTime) == 0) && b.wayOfSelling.ToLower() == wayofselling.ToLower() && b.postingDate >= start && b.receiptDate <= end).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                        }
                    }
                    break;
                case "Oboje":
                    {
                        if (DateTime.Compare(start, defaultDateTime) == 0)
                        {
                            //nije selektovao opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {

                                _logger.LogInformation("nije selektovao opseg niti nacin slanja a prikazati i poslate i primljene");
                                var bills = await _context.Bills.ToListAsync(); //ovo su ustvari svi racuni
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }
                            }
                            else
                            {

                                _logger.LogInformation("nije selektovao opseg, selektovao je nacin slanja a prikazati i poslate i primljene");
                                var bills = await _context.Bills.Where(b=> b.wayOfSelling.ToLower()==wayofselling.ToLower()).ToListAsync(); 
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }

                            }
                        }
                        else
                        {
                            //selektovao je opseg
                            if (string.IsNullOrEmpty(wayofselling))
                            {
                                _logger.LogInformation("selektovao je opseg, nije nacin slanja a prikazati i poslate i primljene");
                                var bills = await _context.Bills.Where(b => b.postingDate >=start && b.receiptDate<=end).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }

                            }
                            else
                            {
                                _logger.LogInformation("selektovao opseg i nacin slanja a prikazati i poslate i primljene");
                                var bills = await _context.Bills.Where(b => b.postingDate >= start && b.receiptDate <= end && b.wayOfSelling.ToLower() == wayofselling.ToLower()).ToListAsync();
                                foreach (var bill in bills)
                                {
                                    _logger.LogInformation("[Poceo je stream osoba prema nacinu slanja]");
                                    await responseStream.WriteAsync(_convert.Convert(bill));
                                }

                            }
                        }
                    }
                    break;
            }



        }
    }
}