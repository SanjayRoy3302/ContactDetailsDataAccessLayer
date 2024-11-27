using ContactDataAccessLayer.Common;
using ContactServiceLayer;
using ContactServiceLayer.Models;
using System;

namespace ContactDataAccessLayer
{
    public class ContactDetailsDAL : IContactDetailsService
    {
        public List<ContactDetails> GetAllContactsList()
        {
            List<ContactDetails> result = JsonFileHelper.ReadFromJsonFile<ContactDetails>();
            return result;
        }

        public BaseResponseModel AddContact(ContactDetails contactDetails)
        {
            BaseResponseModel result = new BaseResponseModel();
            try
            {
                List<ContactDetails> _contactContext = JsonFileHelper.ReadFromJsonFile<ContactDetails>();
                int lastId = _contactContext.LastOrDefault()!.Id;
                contactDetails.Id = lastId + 1;
                _contactContext.Add(contactDetails);
                JsonFileHelper.WriteToJsonFile(_contactContext);
                result.Id = contactDetails.Id;
                result.StatusCode = (int)StatusCode.Added;
                result.StatusMessage = StatusMessage.Added;

            }
            catch (Exception ex)
            {
                result.StatusCode = (int)StatusCode.Failed;
                result.StatusMessage = StatusMessage.Failed;
            }
            return result;
        }

        public BaseResponseModel UpdateContact(ContactDetails contactDetails)
        {
            BaseResponseModel result = new BaseResponseModel();
            try
            {
                List<ContactDetails> _contactContext = JsonFileHelper.ReadFromJsonFile<ContactDetails>();
                ContactDetails? _contactDetail = _contactContext.Where(a => a.Id == contactDetails.Id).FirstOrDefault();
                if (_contactDetail == null)
                {
                    result.StatusCode = (int)StatusCode.NotFound;
                    result.StatusMessage = StatusMessage.NotFound;
                }
                else
                {
                    _contactDetail.Firstname = contactDetails.Firstname;
                    _contactDetail.Lastname = contactDetails.Lastname;
                    _contactDetail.Email = contactDetails.Email;
                    JsonFileHelper.WriteToJsonFile(_contactContext);
                    result.Id = _contactDetail.Id;
                    result.StatusCode = (int)StatusCode.Updated;
                    result.StatusMessage = StatusMessage.Updated;
                }
            }
            catch (Exception)
            {
                result.StatusCode = (int)StatusCode.Failed;
                result.StatusMessage = StatusMessage.Failed;
            }
            return result;
        }

        public BaseResponseModel DeleteContact(int contactDetailId)
        {
            BaseResponseModel result = new BaseResponseModel();
            try
            {
                List<ContactDetails> _contactContext = JsonFileHelper.ReadFromJsonFile<ContactDetails>();
                ContactDetails? contactDetail = _contactContext.Where(a => a.Id == contactDetailId).FirstOrDefault();
                if (contactDetail == null)
                {
                    result.StatusCode = (int)StatusCode.NotFound;
                    result.StatusMessage = StatusMessage.NotFound;
                }
                else
                {
                    _contactContext.Remove(contactDetail);
                    JsonFileHelper.WriteToJsonFile(_contactContext);
                    result.Id = contactDetail.Id;
                    result.StatusCode = (int)StatusCode.Deleted;
                    result.StatusMessage = StatusMessage.Deleted;
                }
            }
            catch (Exception)
            {
                result.StatusCode = (int)StatusCode.Failed;
                result.StatusMessage = StatusMessage.Failed;
            }
            return result;
        }

    }
}
