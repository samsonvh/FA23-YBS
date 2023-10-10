using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;
using YBS.Data.UnitOfWorks;
using YBS.Data.UnitOfWorks.Implements;
using YBS.Services.Dtos;
using YBS.Services.Dtos.InputDtos;
using YBS.Services.Services;

namespace YBS.Service.Services.Implements
{
    public class UpdateRequestService : IUpdateRequestService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        public UpdateRequestService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<UpdateRequestDto> CreateUpdateRequest(UpdateRequestInputDto updateRequestInputDto)
        {
            var company = await _unitOfWorks.CompanyRepository
               .Find(company => company.Id == updateRequestInputDto.CompanyId)
               .FirstOrDefaultAsync();
            if (company != null)
            {
                var updateRequest = new UpdateRequest
                {
                    CompanyId = updateRequestInputDto.CompanyId,
                    Name = updateRequestInputDto.Name,
                    Address = updateRequestInputDto.Address,
                    Hotline = updateRequestInputDto.Hotline,
                    Logo = updateRequestInputDto.Logo,
                    FacebookURL = updateRequestInputDto.FacebookURL,
                    InstagramURL = updateRequestInputDto.InstagramURL,
                    LinkedInURL = updateRequestInputDto.LinkedInURL,
                    Status = EnumCompanyUpdateRequest.PENDING
                };
                _unitOfWorks.UpdateRequestRepository.Add(updateRequest);
                await _unitOfWorks.SaveChangesAsync();
                var updateRequestDto = _mapper.Map<UpdateRequestDto>(updateRequest);
                return updateRequestDto;
            }
            return null;
        }

        public async Task<UpdateRequestDto> GetDetailUpdateRequest(int id)
        {
            var updateRequest = await _unitOfWorks.UpdateRequestRepository
              .Find(updateRequest => updateRequest.Id == id)
              .FirstOrDefaultAsync();
            if (updateRequest != null)
            {
                return _mapper.Map<UpdateRequestDto>(updateRequest);
            }
            return null;
        }

        public async Task<bool> Update(int id, UpdateRequestInputDto updateRequestInputDto)
        {
            var updateRequest = await _unitOfWorks.UpdateRequestRepository
               .Find(updateRequest => updateRequest.Id == id)
               .Include(updateRequest => updateRequest.Company)
               .FirstOrDefaultAsync();

            if (updateRequest == null)
            {
                return false;
            }

            if (updateRequest.Status == EnumCompanyUpdateRequest.PENDING && updateRequestInputDto.Status == EnumCompanyUpdateRequest.APPROVE)
            {
                var companyToUpdate = updateRequest.Company;

                if (updateRequestInputDto.Name != null)
                {
                    companyToUpdate.Name = updateRequestInputDto.Name;
                }

                if (updateRequestInputDto.Address != null)
                {
                    companyToUpdate.Address = updateRequestInputDto.Address;
                }

                if (updateRequestInputDto.Hotline != null)
                {
                    companyToUpdate.HotLine = updateRequestInputDto.Hotline;
                }

                if (updateRequestInputDto.Logo != null)
                {
                    companyToUpdate.Logo = updateRequestInputDto.Logo;
                }

                if (updateRequestInputDto.FacebookURL != null)
                {
                    companyToUpdate.FacebookURL = updateRequestInputDto.FacebookURL;
                }

                if (updateRequestInputDto.InstagramURL != null)
                {
                    companyToUpdate.InstagramURL = updateRequestInputDto.InstagramURL;
                }

                if (updateRequestInputDto.LinkedInURL != null)
                {
                    companyToUpdate.LinkedInURL = updateRequestInputDto.LinkedInURL;
                }

                updateRequest.Status = EnumCompanyUpdateRequest.APPROVE;
                companyToUpdate.Status = EnumCompanyStatus.ACTIVE;

                _unitOfWorks.UpdateRequestRepository.Update(updateRequest);
                await _unitOfWorks.SaveChangesAsync();
                return true;
            }
            else if (updateRequestInputDto.Status == EnumCompanyUpdateRequest.DECLINE)
            {
                updateRequest.Status = EnumCompanyUpdateRequest.DECLINE;
                _unitOfWorks.UpdateRequestRepository.Update(updateRequest);
                await _unitOfWorks.SaveChangesAsync();
                return false;
            }

            return false;
        }

    }
}
