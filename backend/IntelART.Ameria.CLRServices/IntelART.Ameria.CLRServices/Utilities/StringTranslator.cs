using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace IntelART.Ameria.CLRServices
{
    public class StringTranlator
    {
        public static string ToUnicode(string source)
        {
            return TextConvert(Encoding.GetEncoding("Windows-1252"), new ARMSCII8Encoding(), source.Replace(Convert.ToChar(0xa8, CultureInfo.InvariantCulture), Convert.ToChar(0xa2, CultureInfo.InvariantCulture)));
        }

        public static string FromUnicode(string source)
        {
            return TextConvert(new ARMSCII8Encoding(), Encoding.GetEncoding("Windows-1252"), source).Replace(Convert.ToChar(0xa2, CultureInfo.InvariantCulture), Convert.ToChar(0xa8, CultureInfo.InvariantCulture));
        }

        public static string TextConvert(Encoding srcEncoding, Encoding dstEncoding, string source)
        {
            if (srcEncoding == null)
                throw new ArgumentNullException("srcEncoding");
            if (dstEncoding == null)
                throw new ArgumentNullException("dstEncoding");
            if (source == null)
                throw new ArgumentNullException("source");
            if (source == String.Empty)
                return String.Empty;
            return dstEncoding.GetString(srcEncoding.GetBytes(source));
        }
    }

    public class ARMSCII8Encoding : Encoding
    {
        private static Dictionary<int, int> ARMSCII8ToUnicode { get; set; }
        private static Dictionary<int, int> UnicodeToARMSCII8 { get; set; }

        public ARMSCII8Encoding()
            : base(1067)
        {
        }

        static ARMSCII8Encoding()
        {
            ARMSCII8ToUnicode = new Dictionary<int, int>();
            UnicodeToARMSCII8 = new Dictionary<int, int>();

            AddMapping(0, 0); // NUL - NULL
            AddMapping(1, 1); // SOH - START OF HEADER
            AddMapping(2, 2); // STX - START OF TEXT
            AddMapping(3, 3); // ETX - END OF TEXT
            AddMapping(4, 4); // EOT - END OF TRANSMISSION
            AddMapping(5, 5); // ENQ - ENQUIRY
            AddMapping(6, 6); // ACK - ACKNOWLEDGE
            AddMapping(7, 7); // BEL - BELL
            AddMapping(8, 8); // BS - BACKSPACE
            AddMapping(9, 9); // HT - HORIZONTAL TAB
            AddMapping(10, 10); // LF - LINEFEED
            AddMapping(11, 11); // VT - VERTICAL TAB
            AddMapping(12, 12); // FF - FORM FEED
            AddMapping(13, 13); // CR - CARRIAGE RETURN
            AddMapping(14, 14); // SO - SHIFT OUT
            AddMapping(15, 15); // SI - SHIFT IN
            AddMapping(16, 16); // DLE - DATA LINK ESCAPE
            AddMapping(17, 17); // DC1 - DEVICE CONTROL 1 (XON)
            AddMapping(18, 18); // DC2 - DEVICE CONTROL 2
            AddMapping(19, 19); // DC3 - DEVICE CONTROL 3 (XOFF)
            AddMapping(20, 20); // DC4 - DEVICE CONTROL 4
            AddMapping(21, 21); // NAK - NEGATIVE ACKNOWLEDGE
            AddMapping(22, 22); // SYN - SYNCHRONOUS IDLE
            AddMapping(23, 23); // ETB - END OF TRANSMISSION BLOCK
            AddMapping(24, 24); // CAN - CANCEL
            AddMapping(25, 25); // EM - END OF MEDIUM
            AddMapping(26, 26); // SUB - SUBSTITUTE
            AddMapping(27, 27); // ESC - ESCAPE
            AddMapping(28, 28); // FS - FILE SEPARATOR
            AddMapping(29, 29); // GS - GROUP SEPARATOR
            AddMapping(30, 30); // RS - RECORD SEPARATOR
            AddMapping(31, 31); // US - UNIT SEPARATOR

            ; // Characters
            AddMapping(32, 32); // SPACE
            AddMapping(33, 33); // EXCLAMATION MARK
            AddMapping(34, 34); // QUOTATION MARK
            AddMapping(35, 35); // NUMBER SIGN
            AddMapping(36, 36); // DOLLAR SIGN
            AddMapping(37, 37); // PERCENT SIGN
            AddMapping(38, 38); // AMPERSAND
            AddMapping(39, 39); // APOSTROPHE
            AddMapping(40, 40); // LEFT PARENTHESIS
            AddMapping(41, 41); // RIGHT PARENTHESIS
            AddMapping(42, 42); // ASTERISK
            AddMapping(43, 43); // PLUS SIGN
            AddMapping(44, 44); // COMMA
            AddMapping(45, 45); // HYPHEN-MINUS
            AddMapping(46, 46); // FULL STOP
            AddMapping(47, 47); // SOLIDUS
            AddMapping(48, 48); // DIGIT ZERO
            AddMapping(49, 49); // DIGIT ONE
            AddMapping(50, 50); // DIGIT TWO
            AddMapping(51, 51); // DIGIT THREE
            AddMapping(52, 52); // DIGIT FOUR
            AddMapping(53, 53); // DIGIT FIVE
            AddMapping(54, 54); // DIGIT SIX
            AddMapping(55, 55); // DIGIT SEVEN
            AddMapping(56, 56); // DIGIT EIGHT
            AddMapping(57, 57); // DIGIT NINE
            AddMapping(58, 58); // COLON
            AddMapping(59, 59); // SEMICOLON
            AddMapping(60, 60); // LESS-THAN SIGN
            AddMapping(61, 61); // EQUALS SIGN
            AddMapping(62, 62); // GREATER-THAN SIGN
            AddMapping(63, 63); // QUESTION MARK
            AddMapping(64, 64); // COMMERCIAL AT
            AddMapping(65, 65); // LATIN CAPITAL LETTER A
            AddMapping(66, 66); // LATIN CAPITAL LETTER B
            AddMapping(67, 67); // LATIN CAPITAL LETTER C
            AddMapping(68, 68); // LATIN CAPITAL LETTER D
            AddMapping(69, 69); // LATIN CAPITAL LETTER E
            AddMapping(70, 70); // LATIN CAPITAL LETTER F
            AddMapping(71, 71); // LATIN CAPITAL LETTER G
            AddMapping(72, 72); // LATIN CAPITAL LETTER H
            AddMapping(73, 73); // LATIN CAPITAL LETTER I
            AddMapping(74, 74); // LATIN CAPITAL LETTER J
            AddMapping(75, 75); // LATIN CAPITAL LETTER K
            AddMapping(76, 76); // LATIN CAPITAL LETTER L
            AddMapping(77, 77); // LATIN CAPITAL LETTER M
            AddMapping(78, 78); // LATIN CAPITAL LETTER N
            AddMapping(79, 79); // LATIN CAPITAL LETTER O
            AddMapping(80, 80); // LATIN CAPITAL LETTER P
            AddMapping(81, 81); // LATIN CAPITAL LETTER Q
            AddMapping(82, 82); // LATIN CAPITAL LETTER R
            AddMapping(83, 83); // LATIN CAPITAL LETTER S
            AddMapping(84, 84); // LATIN CAPITAL LETTER T
            AddMapping(85, 85); // LATIN CAPITAL LETTER U
            AddMapping(86, 86); // LATIN CAPITAL LETTER V
            AddMapping(87, 87); // LATIN CAPITAL LETTER W
            AddMapping(88, 88); // LATIN CAPITAL LETTER X
            AddMapping(89, 89); // LATIN CAPITAL LETTER Y
            AddMapping(90, 90); // LATIN CAPITAL LETTER Z
            AddMapping(91, 91); // LEFT SQUARE BRACKET
            AddMapping(92, 92); // REVERSE SOLIDUS
            AddMapping(93, 93); // RIGHT SQUARE BRACKET
            AddMapping(94, 94); // CIRCUMFLEX ACCENT
            AddMapping(95, 95); // LOW LINE
            AddMapping(96, 96); // GRAVE ACCENT
            AddMapping(97, 97); // LATIN SMALL LETTER A
            AddMapping(98, 98); // LATIN SMALL LETTER B
            AddMapping(99, 99); // LATIN SMALL LETTER C
            AddMapping(100, 100); // LATIN SMALL LETTER D
            AddMapping(101, 101); // LATIN SMALL LETTER E
            AddMapping(102, 102); // LATIN SMALL LETTER F
            AddMapping(103, 103); // LATIN SMALL LETTER G
            AddMapping(104, 104); // LATIN SMALL LETTER H
            AddMapping(105, 105); // LATIN SMALL LETTER I
            AddMapping(106, 106); // LATIN SMALL LETTER J
            AddMapping(107, 107); // LATIN SMALL LETTER K
            AddMapping(108, 108); // LATIN SMALL LETTER L
            AddMapping(109, 109); // LATIN SMALL LETTER M
            AddMapping(110, 110); // LATIN SMALL LETTER N
            AddMapping(111, 111); // LATIN SMALL LETTER O
            AddMapping(112, 112); // LATIN SMALL LETTER P
            AddMapping(113, 113); // LATIN SMALL LETTER Q
            AddMapping(114, 114); // LATIN SMALL LETTER R
            AddMapping(115, 115); // LATIN SMALL LETTER S
            AddMapping(116, 116); // LATIN SMALL LETTER T
            AddMapping(117, 117); // LATIN SMALL LETTER U
            AddMapping(118, 118); // LATIN SMALL LETTER V
            AddMapping(119, 119); // LATIN SMALL LETTER W
            AddMapping(120, 120); // LATIN SMALL LETTER X
            AddMapping(121, 121); // LATIN SMALL LETTER Y
            AddMapping(122, 122); // LATIN SMALL LETTER Z
            AddMapping(123, 123); // LEFT CURLY BRACKET
            AddMapping(124, 124); // VERTICAL LINE
            AddMapping(125, 125); // RIGHT CURLY BRACKET
            AddMapping(126, 126); // TILDE
            AddMapping(127, 127); // DELETE

            ; //PseudoGraphics sybols
            AddMapping(130, 9484);
            AddMapping(131, 9488);
            AddMapping(132, 9524);
            AddMapping(133, 9508);
            AddMapping(134, 9500);
            AddMapping(135, 9492);
            AddMapping(136, 9496);
            AddMapping(137, 9472);
            AddMapping(139, 9474);
            AddMapping(140, 9516);

            AddMapping(160, 160); // NO-BREAK SPACE

            AddMapping(162, 0x587); // ARMENIAN SMALL LIGATURE ECH YIWN
            AddMapping(163, 0x589); // ARMENIAN FULL STOP
            AddMapping(164, 0x29); // RIGHT PARENTHESIS
            AddMapping(165, 0x28); // LEFT PARENTHESIS
            AddMapping(166, 0xbb); // RIGHT-POINTING DOUBLE ANGLE QUOTATION MARK
            AddMapping(167, 0xab); // LEFT-POINTING DOUBLE ANGLE QUOTATION MARK
            AddMapping(168, 0x2014); // EM DASH
            AddMapping(169, 0x2e); // FULL STOP
            AddMapping(170, 0x55d); // ARMENIAN COMMA
            AddMapping(171, 0x2c); // COMMA
            AddMapping(172, 0x2d); // HYPHEN-MINUS
            AddMapping(173, 0x58a); // ARMENIAN HYPHEN
            AddMapping(174, 0x2026); // HORIZONTAL ELLIPSIS
            AddMapping(175, 0x55c); // ARMENIAN EXCLAMATION MARK
            AddMapping(176, 0x55b); // ARMENIAN EMPHASIS MARK
            AddMapping(177, 0x55e); // ARMENIAN QUESTION MARK
            AddMapping(178, 0x531); // ARMENIAN CAPITAL LETTER AYB
            AddMapping(179, 0x561); // ARMENIAN SMALL LETTER AYB
            AddMapping(180, 0x532); // ARMENIAN CAPITAL LETTER BEN
            AddMapping(181, 0x562); // ARMENIAN SMALL LETTER BEN
            AddMapping(182, 0x533); // ARMENIAN CAPITAL LETTER GIM
            AddMapping(183, 0x563); // ARMENIAN SMALL LETTER GIM
            AddMapping(184, 0x534); // ARMENIAN CAPITAL LETTER DA
            AddMapping(185, 0x564); // ARMENIAN SMALL LETTER DA
            AddMapping(186, 0x535); // ARMENIAN CAPITAL LETTER ECH
            AddMapping(187, 0x565); // ARMENIAN SMALL LETTER ECH
            AddMapping(188, 0x536); // ARMENIAN CAPITAL LETTER ZA
            AddMapping(189, 0x566); // ARMENIAN SMALL LETTER ZA
            AddMapping(190, 0x537); // ARMENIAN CAPITAL LETTER EH
            AddMapping(191, 0x567); // ARMENIAN SMALL LETTER EH
            AddMapping(192, 0x538); // ARMENIAN CAPITAL LETTER ET
            AddMapping(193, 0x568); // ARMENIAN SMALL LETTER ET
            AddMapping(194, 0x539); // ARMENIAN CAPITAL LETTER TO
            AddMapping(195, 0x569); // ARMENIAN SMALL LETTER TO
            AddMapping(196, 0x53a); // ARMENIAN CAPITAL LETTER ZHE
            AddMapping(197, 0x56a); // ARMENIAN SMALL LETTER ZHE
            AddMapping(198, 0x53b); // ARMENIAN CAPITAL LETTER INI
            AddMapping(199, 0x56b); // ARMENIAN SMALL LETTER INI
            AddMapping(200, 0x53c); // ARMENIAN CAPITAL LETTER LIWN
            AddMapping(201, 0x56c); // ARMENIAN SMALL LETTER LIWN
            AddMapping(202, 0x53d); // ARMENIAN CAPITAL LETTER XEH
            AddMapping(203, 0x56d); // ARMENIAN SMALL LETTER XEH
            AddMapping(204, 0x53e); // ARMENIAN CAPITAL LETTER CA
            AddMapping(205, 0x56e); // ARMENIAN SMALL LETTER CA
            AddMapping(206, 0x53f); // ARMENIAN CAPITAL LETTER KEN
            AddMapping(207, 0x56f); // ARMENIAN SMALL LETTER KEN
            AddMapping(208, 0x540); // ARMENIAN CAPITAL LETTER HO
            AddMapping(209, 0x570); // ARMENIAN SMALL LETTER HO
            AddMapping(210, 0x541); // ARMENIAN CAPITAL LETTER JA
            AddMapping(211, 0x571); // ARMENIAN SMALL LETTER JA
            AddMapping(212, 0x542); // ARMENIAN CAPITAL LETTER GHAD
            AddMapping(213, 0x572); // ARMENIAN SMALL LETTER GHAD
            AddMapping(214, 0x543); // ARMENIAN CAPITAL LETTER CHEH
            AddMapping(215, 0x573); // ARMENIAN SMALL LETTER CHEH
            AddMapping(216, 0x544); // ARMENIAN CAPITAL LETTER MEN
            AddMapping(217, 0x574); // ARMENIAN SMALL LETTER MEN
            AddMapping(218, 0x545); // ARMENIAN CAPITAL LETTER YI
            AddMapping(219, 0x575); // ARMENIAN SMALL LETTER YI
            AddMapping(220, 0x546); // ARMENIAN CAPITAL LETTER NOW
            AddMapping(221, 0x576); // ARMENIAN SMALL LETTER NOW
            AddMapping(222, 0x547); // ARMENIAN CAPITAL LETTER SHA
            AddMapping(223, 0x577); // ARMENIAN SMALL LETTER SHA
            AddMapping(224, 0x548); // ARMENIAN CAPITAL LETTER VO
            AddMapping(225, 0x578); // ARMENIAN SMALL LETTER VO
            AddMapping(226, 0x549); // ARMENIAN CAPITAL LETTER CHA
            AddMapping(227, 0x579); // ARMENIAN SMALL LETTER CHA
            AddMapping(228, 0x54a); // ARMENIAN CAPITAL LETTER PEH
            AddMapping(229, 0x57a); // ARMENIAN SMALL LETTER PEH
            AddMapping(230, 0x54b); // ARMENIAN CAPITAL LETTER JHEH
            AddMapping(231, 0x57b); // ARMENIAN SMALL LETTER JHEH
            AddMapping(232, 0x54c); // ARMENIAN CAPITAL LETTER RA
            AddMapping(233, 0x57c); // ARMENIAN SMALL LETTER RA
            AddMapping(234, 0x54d); // ARMENIAN CAPITAL LETTER SEH
            AddMapping(235, 0x57d); // ARMENIAN SMALL LETTER SEH
            AddMapping(236, 0x54e); // ARMENIAN CAPITAL LETTER VEW
            AddMapping(237, 0x57e); // ARMENIAN SMALL LETTER VEW
            AddMapping(238, 0x54f); // ARMENIAN CAPITAL LETTER TIWN
            AddMapping(239, 0x57f); // ARMENIAN SMALL LETTER TIWN
            AddMapping(240, 0x550); // ARMENIAN CAPITAL LETTER REH
            AddMapping(241, 0x580); // ARMENIAN SMALL LETTER REH
            AddMapping(242, 0x551); // ARMENIAN CAPITAL LETTER CO
            AddMapping(243, 0x581); // ARMENIAN SMALL LETTER CO
            AddMapping(244, 0x552); // ARMENIAN CAPITAL LETTER YIWN
            AddMapping(245, 0x582); // ARMENIAN SMALL LETTER YIWN
            AddMapping(246, 0x553); // ARMENIAN CAPITAL LETTER PIWR
            AddMapping(247, 0x583); // ARMENIAN SMALL LETTER PIWR
            AddMapping(248, 0x554); // ARMENIAN CAPITAL LETTER KEH
            AddMapping(249, 0x584); // ARMENIAN SMALL LETTER KEH
            AddMapping(250, 0x555); // ARMENIAN CAPITAL LETTER OH
            AddMapping(251, 0x585); // ARMENIAN SMALL LETTER OH
            AddMapping(252, 0x556); // ARMENIAN CAPITAL LETTER FEH
            AddMapping(253, 0x586); // ARMENIAN SMALL LETTER FEH
            AddMapping(254, 0x55a); // ARMENIAN APOSTROPHE
        }

        private static void AddMapping(int armscii8Point, int unicodePoint)
        {
            if (armscii8Point < 0 || armscii8Point > 255)
                throw new ArgumentOutOfRangeException("armscii8Point");
            if (unicodePoint < (int)Char.MinValue || unicodePoint > (int)Char.MaxValue)
                throw new ArgumentOutOfRangeException("unicodePoint");

            if (!ARMSCII8ToUnicode.ContainsKey(armscii8Point))
                ARMSCII8ToUnicode.Add(armscii8Point, unicodePoint);
            if (!UnicodeToARMSCII8.ContainsKey(unicodePoint))
                UnicodeToARMSCII8.Add(unicodePoint, armscii8Point);
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");
            if (index < chars.GetLowerBound(0) || index > chars.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("index");
            if (count < 0 || index + count - 1 > chars.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("count");

            int result = 0;
            while (count > 0)
            {
                count--;
                int unicodePoint = (int)chars[index];
                index++;
                if (UnicodeToARMSCII8.ContainsKey(unicodePoint))
                    result++;
            }
            return result;
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (chars == null)
                throw new ArgumentNullException("chars");
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (charIndex < chars.GetLowerBound(0) || charIndex > chars.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("charIndex");
            if (charCount < 0 || charIndex + charCount - 1 > chars.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("charCount");
            if (chars.Length > 1 && (byteIndex < bytes.GetLowerBound(0) || byteIndex > bytes.GetUpperBound(0)))
                throw new ArgumentOutOfRangeException("byteIndex");

            int result = 0;
            for (int index = 0; index < charCount; index++)
            {
                int unicodePoint = (int)chars[charIndex + index];
                if (UnicodeToARMSCII8.ContainsKey(unicodePoint))
                {
                    int armscii8Point = UnicodeToARMSCII8[unicodePoint];
                    if (byteIndex > bytes.GetUpperBound(0))
                        throw new ArgumentOutOfRangeException("bytes");
                    bytes[byteIndex] = (byte)armscii8Point;
                    byteIndex++;
                    result++;
                }
            }
            return result;
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (index < bytes.GetLowerBound(0) || index > bytes.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("index");
            if (count < 0 || index + count - 1 > bytes.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("count");

            int result = 0;
            while (count > 0)
            {
                count--;
                int armscii8Point = (int)bytes[index];
                index++;
                if (ARMSCII8ToUnicode.ContainsKey(armscii8Point))
                    result++;
            }
            return result;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (chars == null)
                throw new ArgumentNullException("chars");
            if (byteIndex < bytes.GetLowerBound(0) || byteIndex > bytes.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("byteIndex");
            if (byteCount < 0 || byteIndex + byteCount - 1 > bytes.GetUpperBound(0))
                throw new ArgumentOutOfRangeException("byteCount");
            if (bytes.Length > 1 && (charIndex < chars.GetLowerBound(0) || charIndex > chars.GetUpperBound(0)))
                throw new ArgumentOutOfRangeException("charIndex");

            int result = 0;
            for (int index = 0; index < byteCount; index++)
            {
                int armscii8Point = (int)bytes[byteIndex + index];
                if (ARMSCII8ToUnicode.ContainsKey(armscii8Point))
                {
                    int unicodePoint = ARMSCII8ToUnicode[armscii8Point];
                    if (charIndex > chars.GetUpperBound(0))
                        throw new ArgumentOutOfRangeException("chars");
                    chars[charIndex] = (char)unicodePoint;
                    charIndex++;
                    result++;
                }
            }
            return result;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        public override string BodyName
        {
            get
            {
                return "armscii-8";
            }
        }

        public override string HeaderName
        {
            get
            {
                return BodyName;
            }
        }

        public override string EncodingName
        {
            get
            {
                return "ARMSCII-8";
            }
        }

        public override int WindowsCodePage
        {
            get
            {
                return 1252;
            }
        }

        public override bool IsBrowserDisplay
        {
            get
            {
                return false;
            }
        }

        public override bool IsBrowserSave
        {
            get
            {
                return false;
            }
        }

        public override bool IsMailNewsDisplay
        {
            get
            {
                return false;
            }
        }

        public override bool IsMailNewsSave
        {
            get
            {
                return false;
            }
        }
    }
}
