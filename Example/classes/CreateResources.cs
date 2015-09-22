using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hl7.Fhir.Model;

namespace hscic.gov.uk.fhir.interop.fgm
{
    internal static class CreateResources
    {
        internal static Parameters CreateQueryResource()
        {
            // Create new parameter resource
            Parameters res = new Parameters();

            // Allocate the logical resource Id - this is what the resource is referenced by
            res.Id = "7cb73a48-090d-469a-a2b2-04f1e6b11ea2";

            // Add the profile for this resource (from the FGM DMS)
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-QueryParameters" };
            res.Meta = metadata;

            // Add the first query parameter, this will always be this value (from the FGM DMS)
            Parameters.ParametersParameterComponent p1 = new Parameters.ParametersParameterComponent();
            p1.Name = "RiskIndicator";
            p1.Value = new FhirString("FGM");
            res.Parameter.Add(p1);

            // Add the second query parameter, the patient's NHS Number (from the FGM DMS)
            Parameters.ParametersParameterComponent p2 = new Parameters.ParametersParameterComponent();
            p2.Name = "NHSNumber";
            p2.Value = new FhirString("9999999999");
            res.Parameter.Add(p2);

            return res;
        }

        internal static Organization CreateOrganizationResource()
        {
            // Create new Organization resource
            Organization res = new Organization();

            // Allocate the logical resource Id - this is what the resource is referenced by
            res.Id = "13daadee-26e1-4d6a-9e6a-7f4af9b58878";

            // Add the profile for this resource (from the FGM DMS)
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-Organization" };
            res.Meta = metadata;

            // Add the business idetifier for the organisation, plus the organisation name
            res.Identifier = new List<Identifier>();
            Identifier id = new Identifier("urn:fhir.nhs.uk/id/ODSOrganisationCode", "RKE");
            res.Identifier.Add(id);
            res.Name = "THE WHITTINGTON HOSPITAL NHS TRUST";

            return res;
        }

        internal static Practitioner CreatePractitionerResource()
        {
            // Create new Practitioner resource
            Practitioner res = new Practitioner();

            // Allocate the logical resource Id - this is what the resource is referenced by
            res.Id = "41fe704c-18e5-11e5-b60b-1697f925ec7b";

            // Add the profile for this resource (from the FGM DMS)
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-Practitioner" };
            res.Meta = metadata;

            // Add the business idetifier for the SDS User Id
            res.Identifier = new List<Identifier>();
            Identifier id1 = new Identifier("urn:fhir.nhs.uk/id/SDSUserID", "G12345678");
            id1.Use = Identifier.IdentifierUse.Official;
            res.Identifier.Add(id1);

            // Add the business idetifier for the SDS Role Profile Id
            Identifier id2 = new Identifier("urn:fhir.nhs.uk/id/SDSRoleProfileID", "PT1234");
            id2.Use = Identifier.IdentifierUse.Official;
            res.Identifier.Add(id2);

            // Add the name of the practitioner
            res.Name = HumanName.ForFamily("Wood").WithGiven("Town");
            res.Name.Use = HumanName.NameUse.Official;
            res.Name.Prefix = new String[] { "Dr." };

            // Add details about the practitioner's role
            Practitioner.PractitionerPractitionerRoleComponent pr = new Practitioner.PractitionerPractitionerRoleComponent();
            Coding code = new Coding("urn:fhir.nhs.uk:vs/SDSJobRoleName", "R0090");
            code.Display = "Hospital Practitioner";

            pr.Role = new CodeableConcept();
            pr.Role.Coding = new List<Coding>();
            pr.Role.Coding.Add(code);

            // Add details about the practitioner's managing organisation
            pr.ManagingOrganization =
                new ResourceReference()
                { Reference = "Organization/41fe704c-18e5-11e5-b60b-1697f925ec7b" };

            res.PractitionerRole.Add(pr);
            return res;
        }

        internal static MessageHeader CreateMessageHeaderResource()
        {
            // Create new MessageHeader resource
            MessageHeader res = new MessageHeader();

            // Allocate the logical resource Id - this is what the resource is referenced by
            res.Id = "14daadee-26e1-4d6a-9e6a-7f4af9b58877";

            // Add the profile for this resource (from the FGM DMS) plus a timestamp for when the resource was last updated.
            Meta metadata = new Meta();
            metadata.Profile = new string[] { "urn:fhir.nhs.uk:profile/NHS-FGM-MessageHeader-QueryParameters" };
            metadata.LastUpdated = new DateTimeOffset(2015, 6, 22, 14, 04, 44, new TimeSpan(0));
            res.Meta = metadata;

            // Add the business idetifier for the MessageHeader
            res.Identifier = "13daadee-26e1-4d6a-9e6a-7f4af9b58977";

            // Add a timestamp for the message
            res.Timestamp = new DateTimeOffset(2015, 7, 4, 9, 10, 14, new TimeSpan(0));

            // Add an event code (as per the FGM DMS)
            res.Event = new Coding()
            {
                System = "urn:fhir.nhs.uk:vs/MessageEvent",
                Code = "urn:nhs:names:services:fgmquery/FGMQuery_1_0"
            };

            // Add details of the endpoint sending the message
            res.Source = new MessageHeader.MessageSourceComponent()
            {
                Name = "FooBar NHS Trust",
                Software = "FooBar Patient Manager",
                Contact = new ContactPoint()
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = "0207 444777"
                },
                Endpoint = "urn:system:asid/047192794544"
            };

            // Add details of the endpoint receiving the message (in this case SPINE 2)
            res.Destination = new List<MessageHeader.MessageDestinationComponent>();
            res.Destination.Add(
                new MessageHeader.MessageDestinationComponent()
                {
                    Name = "SPINE 2 MHS",
                    Endpoint = "urn:spinecore:asid/990101234567"
                });

            // Add a reference to a Practitioner resource detailing the message author
            res.Author = new ResourceReference()
            {
                Reference = "Practitioner/41fe704c-18e5-11e5-b60b-1697f925ec7b",
                Display = "Dr Town Wood"
            };

            // Add a reference to the resource that constitutes the message payload
            res.Data = new List<ResourceReference>();
            res.Data.Add(new ResourceReference() { Reference = "Parameters/7cb73a48-090d-469a-a2b2-04f1e6b11ea2" });
            return res;
        }

    }
}

