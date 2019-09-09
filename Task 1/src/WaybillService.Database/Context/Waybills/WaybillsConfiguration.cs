using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WaybillService.Database.Context.Waybills.Models;
using WaybillService.Database.Context.Waybills.Models.ValidationResult;

namespace WaybillService.Database.Context.Waybills
{
    public static class WaybillsConfiguration
    {
        public static void ConfigureWaybills(ModelBuilder modelBuilder)
        {
            ConfigureWaybillFiles(modelBuilder);
            ConfigureWaybillDocuments(modelBuilder);
            ConfigureEnumDtos(modelBuilder);
        }

        private static void ConfigureWaybillFiles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WaybillFileDto>(entity =>
            {
                entity.ToTable("waybill_files");
                entity.HasKey(e => e.CephId);

                entity.Property(e => e.CephId)
                    .HasColumnName("ceph_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.UploadTime)
                    .HasColumnName("upload_time_ticks")
                    .HasConversion(
                        time => time.UtcTicks,
                        ticks => new DateTimeOffset(ticks, TimeSpan.Zero));
                entity.Property(e => e.DbDisplayedUploadTime)
                    .HasColumnName("upload_time");
            });
        }

        private static void ConfigureWaybillDocuments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WaybillDto>(entity =>
            {
                entity.ToTable("waybills");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id");
                
                entity.Property(e => e.SourceFileId)
                    .HasColumnName("source_file_id");

                entity.Property(e => e.InteractionState)
                    .HasColumnName("interaction_state_id");
                
                entity.Ignore(e => e.InteractionState);
                entity.HasOne<WaybillInteractionStateDto>()
                    .WithMany()
                    .HasForeignKey(x => x.InteractionState)
                    .HasPrincipalKey(x => x.Id)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.File)
                    .WithOne()
                    .HasForeignKey<WaybillDto>(x => x.SourceFileId)
                    .HasPrincipalKey<WaybillFileDto>(x => x.CephId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Content)
                    .WithOne()
                    .HasForeignKey<WaybillContentDto>(x => x.SupplierWaybillId)
                    .HasPrincipalKey<WaybillDto>(x => x.Id)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ValidationResult)
                    .WithOne()
                    .HasForeignKey<WaybillValidationResultDto>(x => x.SupplierWaybillId)
                    .HasPrincipalKey<WaybillDto>(x => x.Id)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WaybillContentDto>(entity =>
            {
                entity.ToTable("waybill_contents");
                entity.HasKey(e => e.SupplierWaybillId);

                entity.Property(e => e.SupplierWaybillId)
                    .HasColumnName("waybill_id");

                entity.Property(e => e.DocumentNumber)
                    .HasColumnName("document_number");
                entity.Property(e => e.DocumentDate)
                    .HasColumnName("document_date");
                entity.Property(e => e.Consignee)
                    .HasColumnName("consignee");

                entity.HasMany(e => e.Items)
                    .WithOne()
                    .HasForeignKey(itemDto => itemDto.SupplierWaybillId)
                    .HasPrincipalKey(contentDto => contentDto.SupplierWaybillId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.TotalVat)
                    .HasColumnName("total_vat_sum");
                entity.Property(e => e.TotalSumWithVat)
                    .HasColumnName("total_sum_with_vat");
                entity.Property(e => e.TotalSumWithoutVat)
                    .HasColumnName("total_sum_without_vat");
            });

            modelBuilder.Entity<WaybillItemDto>(entity =>
            {
                entity.ToTable("waybill_items");
                entity.HasKey(e => new {e.SequenceNumber, e.SupplierWaybillId});

                entity.Property(e => e.SequenceNumber)
                    .HasColumnName("sequence_number");
                entity.Property(e => e.SupplierWaybillId)
                    .HasColumnName("supplier_waybill_id");

                entity.Property(e => e.RezonItemId)
                    .HasColumnName("rezon_item_id");
                entity.Property(e => e.Code)
                    .HasColumnName("code");
                entity.Property(e => e.Name)
                    .HasColumnName("name");
                entity.Property(e => e.Unit)
                    .HasColumnName("unit");
                entity.Property(e => e.UnitOkeiCode)
                    .HasColumnName("unit_okei_code");
                entity.Property(e => e.Amount)
                    .HasColumnName("amount");

                entity.Property(e => e.PriceWithoutVat)
                    .HasColumnName("price_without_vat");
                entity.Property(e => e.VatPercent)
                    .HasColumnName("vat_percent");
                entity.Property(e => e.VatSum)
                    .HasColumnName("vat_sum");
                entity.Property(e => e.SumWithoutVat)
                    .HasColumnName("sum_without_vat");
                entity.Property(e => e.SumWithVat)
                    .HasColumnName("sum_with_vat");

                entity.Property(e => e.CountryOfOriginCode)
                    .HasColumnName("country_of_origin_code");
                entity.Property(e => e.CountryOfOriginName)
                    .HasColumnName("country_of_origin_name");
                entity.Property(e => e.CustomsDeclarationNumber)
                    .HasColumnName("customs_declaration_number");
            });

            modelBuilder.Entity<WaybillValidationResultDto>(entity =>
            {
                entity.ToTable("waybill_validation_result");
                entity.HasKey(e => e.SupplierWaybillId);

                entity.Property(e => e.SupplierWaybillId)
                    .HasColumnName("waybill_id");

                entity.Property(e => e.HasConsigneeError)
                    .HasColumnName("has_consignee_error");
                entity.Property(e => e.HasDocumentDateError)
                    .HasColumnName("has_document_date_error");
                entity.Property(e => e.HasDocumentNumberError)
                    .HasColumnName("has_document_number_error");
                entity.Property(e => e.HasEmptySumWithoutVat)
                    .HasColumnName("has_empty_sum_without_vat");
                entity.Property(e => e.HasEmptyTotalVatSum)
                    .HasColumnName("has_empty_total_vat_sum");
                entity.Property(e => e.HasEmptySumWithVat)
                    .HasColumnName("has_empty_sum_with_vat");
                entity.Property(e => e.HasInvalidSumWithoutVat)
                    .HasColumnName("has_invalid_sum_without_vat");
                entity.Property(e => e.HasInvalidTotalVatSum)
                    .HasColumnName("has_invalid_total_vat_sum");
                entity.Property(e => e.HasInvalidSumWithVat)
                    .HasColumnName("has_invalid_sum_with_vat");

                entity.HasMany(e => e.ItemValidationResults)
                    .WithOne()
                    .HasForeignKey(x => x.WaybillId)
                    .HasPrincipalKey(x => x.SupplierWaybillId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WaybillItemValidationResultDto>(entity =>
            {
                entity.ToTable("waybill_item_validation_result");
                entity.HasKey(e => new {e.WaybillItemSequenceNumber, SupplierWaybillId = e.WaybillId});

                entity.Property(e => e.WaybillItemSequenceNumber)
                    .HasColumnName("waybill_item_ordering_number");
                entity.Property(e => e.WaybillId)
                    .HasColumnName("waybill_id");

                entity.Property(e => e.HasEmptyItemId)
                    .HasColumnName("has_empty_item_id");
                entity.Property(e => e.HasEmptyCode)
                    .HasColumnName("has_empty_code");
                entity.Property(e => e.HasEmptyName)
                    .HasColumnName("has_empty_name");
                entity.Property(e => e.HasEmptyOkeiUnitCode)
                    .HasColumnName("has_empty_okei_unit_code");
                entity.Property(e => e.HasEmptyUnitName)
                    .HasColumnName("has_empty_unit_name");
                entity.Property(e => e.HasEmptyAmount)
                    .HasColumnName("has_empty_amount");
                entity.Property(e => e.HasEmptyPriceWithoutVat)
                    .HasColumnName("has_empty_price_without_vat");
                entity.Property(e => e.HasInvalidVatPercent)
                    .HasColumnName("has_invalid_vat_percent");
                entity.Property(e => e.HasEmptySumWithVat)
                    .HasColumnName("has_empty_sum_with_vat");
                entity.Property(e => e.HasEmptySumWithoutVat)
                    .HasColumnName("has_empty_sum_without_vat");
                entity.Property(e => e.HasInvalidVatSum)
                    .HasColumnName("has_invalid_vat_sum");
                entity.Property(e => e.HasInvalidSumWithVat)
                    .HasColumnName("has_invalid_sum_with_vat");
                entity.Property(e => e.HasEmptyCustomsDeclarationNumber)
                    .HasColumnName("has_empty_customs_declaration_number");
                entity.Property(e => e.HasEmptyCountryOfOriginCode)
                    .HasColumnName("has_empty_country_of_origin_code");
                entity.Property(e => e.HasEmptyCountryOfOriginName)
                    .HasColumnName("has_empty_country_of_origin_name");
            });
        }

        private static void ConfigureEnumDtos(ModelBuilder modelBuilder)
        {
            ConfigureEnumDto<WaybillTypeEnumDto, WaybillTypeDto>(
                modelBuilder, "waybill_types");
            ConfigureEnumDto<WaybillInteractionStateEnumDto, WaybillInteractionStateDto>(
                modelBuilder, "waybill_interaction_states");
        }

        private static void ConfigureEnumDto<TEnum, TDto>(ModelBuilder modelBuilder, string tableName)
            where TEnum : Enum
            where TDto : EnumValuesDto<TEnum>, new()
        {
            modelBuilder.Entity<TDto>(entity =>
            {
                entity.ToTable(tableName);
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();
                entity.Property(e => e.Name).HasColumnName("name");

                var allEnumValues = (TEnum[]) Enum.GetValues(typeof(TEnum));
                var allDtos = allEnumValues
                    .Select(@enum => new TDto()
                    {
                        Id = @enum,
                        Name = @enum.ToString("G"),
                    })
                    .OrderBy(dto => dto.Id);

                entity.HasData(allDtos);
            });
        }
    }
}