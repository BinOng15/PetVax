﻿using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IPetService
    {
        Task<DynamicResponse<PetResponseDTO>> GetAllPetsAsync(GetAllPetsRequestDTO getAllPetsRequest, CancellationToken cancellationToken);
        Task<BaseResponse<PetResponseDTO>> UpdatePetAsync(int petId, UpdatePetRequestDTO updatePetRequest, CancellationToken cancellationToken);
        Task<BaseResponse<PetResponseDTO>> GetPetByIdAsync(int petId, CancellationToken cancellationToken);
        Task<BaseResponse<PetResponseDTO>> CreatePetAsync(CreatePetRequestDTO createPetRequest, CancellationToken cancellationToken);
        Task<List<BaseResponse<PetResponseDTO>>> GetPetsByCustomerIdAsync(int accountId, CancellationToken cancellationToken);
        Task<BaseResponse<PetResponseDTO>> DeletePetById(int petId, CancellationToken cancellationToken);
    }
}
