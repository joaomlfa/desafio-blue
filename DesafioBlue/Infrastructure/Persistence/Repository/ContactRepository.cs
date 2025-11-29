using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Application.UseCases.Commons;
using DesafioBlue.Domain.Entity;
using DesafioBlue.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DesafioBlue.Infrastructure.Persistence.Repository
{
    public class ContactRepository(ContactContext context) : BaseRepository, IContactRepository
    {
        private readonly ContactContext _context = context;

        public async Task<CustomResponse<Contact>> CreateContactAsync(Contact contact)
        {
            try
            {
                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();
                return new CustomResponse<Contact>(statusCode: StatusCodes.Status201Created, message: "Created", data: contact);

            }
            catch (Exception ex)
            {
                return new CustomResponse<Contact>(statusCode: StatusCodes.Status500InternalServerError, message: ex.Message, data: null);
            }
        }

        public async Task<CustomResponse<bool>> DeleteContactAsync(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return new CustomResponse<bool>(statusCode: StatusCodes.Status404NotFound, message: "Contact not found", data: false);
                }

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(statusCode: StatusCodes.Status200OK, message: "Deleted", data: true);
            }
            catch (Exception ex)
            {
                return new CustomResponse<bool>(statusCode: StatusCodes.Status500InternalServerError, message: ex.Message, data: false);
            }
        }

        public async Task<CustomResponse<ListPaginated<Contact>>> GetAllContactsPaginatedAsync(int pageSize, int cursorAfter)
        {
            try
            {
                var query = _context.Contacts
                .Where(bp => bp.Id > cursorAfter)
                .OrderBy(bp => bp.Id);

                var items = await query.Take(GetPageSize(pageSize)).ToListAsync();

                return new CustomResponse<ListPaginated<Contact>>(statusCode: StatusCodes.Status200OK, message: "Ok", data: GetPaginatedResult(items, pageSize));
            }
            catch (Exception ex)
            {

                return new CustomResponse<ListPaginated<Contact>>(statusCode: StatusCodes.Status500InternalServerError, message: ex.Message, data: null);

            }
        }

        public async Task<CustomResponse<Contact>> UpdateContactAsync(Contact contact)
        {
            try
            {
                var existingContact = await _context.Contacts.FindAsync(contact.Id);
                if (existingContact == null)
                {
                    return new CustomResponse<Contact>(statusCode: StatusCodes.Status404NotFound, message: "Contact not found", data: null);
                }

                _context.Entry(existingContact).CurrentValues.SetValues(contact);
                await _context.SaveChangesAsync();
                return new CustomResponse<Contact>(statusCode: StatusCodes.Status200OK, message: "Updated", data: contact);
            }
            catch (Exception ex)
            {
                return new CustomResponse<Contact>(statusCode: StatusCodes.Status500InternalServerError, message: ex.Message, data: null);
            }
        }
    }
}