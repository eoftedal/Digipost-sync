using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Digipostsync.Core;

namespace Digipostsync.Test
{
    [TestFixture]
    public class JSONHelperTest
    {
        [Test]
        public void ShouldSerializeSingle()
        {
            String s = "{\"autentiseringsnivaa\":1,\"logo\":false,\"avsender\":\"Opplastet\",\"adresse\":null,\"arkiverUri\":\"https://www.digipost.no/post/privat/brev/116910/arkiver\",\"tilKjokkenbenkUri\":\"https://www.digipost.no/post/privat/brev/116910/tilkjokkenbenk\",\"avsenderKontoId\":7240,\"emne\":\"The.Beginners.Guide.to.XSS\",\"uri\":\"https://www.digipost.no/post/privat/brev/116910\",\"selvopplastet\":true,\"logoUri\":\"https://www.digipost.no/post/privat/virksomhet/7240/logo.image\",\"brevId\":116910,\"mottattTidspunkt\":\"29.10.2011\",\"brevUri\":\"https://www.digipost.no/post/privat/brev/116910.pdf\",\"lest\":false}";
            var f = JSONHelper.Deserialise<DigipostFile>(s);
            Assert.AreEqual("https://www.digipost.no/post/privat/brev/116910.pdf", f.BrevUri);

        }
        [Test]
        public void ShouldSerialize()
        {
            String s = "[{\"autentiseringsnivaa\":1,\"logo\":false,\"avsender\":\"Opplastet\",\"adresse\":null,\"arkiverUri\":\"https://www.digipost.no/post/privat/brev/116910/arkiver\",\"tilKjokkenbenkUri\":\"https://www.digipost.no/post/privat/brev/116910/tilkjokkenbenk\",\"avsenderKontoId\":7240,\"emne\":\"The.Beginners.Guide.to.XSS\",\"uri\":\"https://www.digipost.no/post/privat/brev/116910\",\"selvopplastet\":true,\"logoUri\":\"https://www.digipost.no/post/privat/virksomhet/7240/logo.image\",\"brevId\":116910,\"mottattTidspunkt\":\"29.10.2011\",\"brevUri\":\"https://www.digipost.no/post/privat/brev/116910.pdf\",\"lest\":false},{\"autentiseringsnivaa\":1,\"logo\":false,\"avsender\":\"Opplastet\",\"adresse\":null,\"arkiverUri\":\"https://www.digipost.no/post/privat/brev/116909/arkiver\",\"tilKjokkenbenkUri\":\"https://www.digipost.no/post/privat/brev/116909/tilkjokkenbenk\",\"avsenderKontoId\":7240,\"emne\":\"Evolutionary.Fuzzing\",\"uri\":\"https://www.digipost.no/post/privat/brev/116909\",\"selvopplastet\":true,\"logoUri\":\"https://www.digipost.no/post/privat/virksomhet/7240/logo.image\",\"brevId\":116909,\"mottattTidspunkt\":\"29.10.2011\",\"brevUri\":\"https://www.digipost.no/post/privat/brev/116909.pdf\",\"lest\":false}]";
            var f = JSONHelper.Deserialise<DigipostFile[]>(s).ToList();
            Assert.AreEqual("https://www.digipost.no/post/privat/brev/116910.pdf", f[0].BrevUri);

        }
    }
}
