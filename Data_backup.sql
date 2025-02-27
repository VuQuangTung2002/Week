PGDMP  -                    |            Data_backup    16.4    16.4 H    =           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            >           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            ?           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            @           1262    24599    Data_backup    DATABASE     �   CREATE DATABASE "Data_backup" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Vietnamese_Vietnam.1252';
    DROP DATABASE "Data_backup";
                postgres    false            �            1255    41066 ^   add_user_with_role(character varying, character varying, character varying, character varying) 	   PROCEDURE     �  CREATE PROCEDURE public.add_user_with_role(IN p_username character varying, IN p_email character varying, IN p_password character varying, IN p_role_name character varying)
    LANGUAGE plpgsql
    AS $$
declare 
v_user_id int;
v_role_id int;
begin
insert into "Users"(
 "UserName","Email","PasswordHash","AccessFailedCount",
 "ConcurrencyStamp","PhoneNumber","PhoneNumberConfirmed",
 "EmailConfirmed","LockoutEnabled","LockoutEnd",
 "TwoFactorEnabled","NormalizedEmail","NormalizedUserName",
 "SecurityStamp"
)
values(p_username, p_email, p_password,0,'0','0',false, false, false, null, false, p_email, p_email, 'MGYZTW6AH4XKCBDFSHJH6DMSMOXSODTA')
returning "Id" into v_user_id;

SELECT "RoleId" INTO v_role_id FROM "Role" WHERE "Name" = p_role_Name;
    IF v_role_id IS NOT NULL THEN
        INSERT INTO "Role" ("Name", "NormalizedName", "ConcurrencyStamp", "Id")
        VALUES (p_role_name, UPPER(p_role_name), '1222', v_user_id)
    returning "RoleId" into v_role_id;
	END IF;

	if p_role_Name  = 'User' then
INSERT INTO "RolePermission"("PermissionId", "RoleId")
    VALUES (8, v_role_id), 
	(14, v_role_id), (7, v_role_id), (15, v_role_id), (1, v_role_id);
	
ELSIF p_role_Name  = 'Admin' then
INSERT INTO "RolePermission"("PermissionId", "RoleId")
    VALUES (1, v_role_id), 
	(2, v_role_id), (4, v_role_id), (5, v_role_id), (7, v_role_id),
	(8, v_role_id), (9, v_role_id),(11, v_role_id), (12, v_role_id), 
	(14, v_role_id), (15, v_role_id);
  end if;
commit;
end;
$$;
 �   DROP PROCEDURE public.add_user_with_role(IN p_username character varying, IN p_email character varying, IN p_password character varying, IN p_role_name character varying);
       public          postgres    false            �            1255    41079 )   add_with_role(integer, character varying) 	   PROCEDURE     z  CREATE PROCEDURE public.add_with_role(IN user_id integer, IN namerole character varying)
    LANGUAGE plpgsql
    AS $$
declare v_role_id int;
begin
Insert into "Role"("Name","NormalizedName","ConcurrencyStamp","Id")
values(NameRole, Upper(NameRole),'1234', user_id);

select "RoleId" into v_role_id from "Role" where "Id" = user_id and "Name" = NameRole;
if NameRole = 'Admin' and v_role_id is not null then
INSERT INTO "RolePermission"("PermissionId", "RoleId")
    VALUES (1, v_role_id), 
	(2, v_role_id), (4, v_role_id), (5, v_role_id), (7, v_role_id),
	(8, v_role_id), (9, v_role_id),(11, v_role_id), (12, v_role_id), 
	(14, v_role_id), (15, v_role_id);
ELSIF NameRole = 'User' and v_role_id is not null then
INSERT INTO "RolePermission"("PermissionId", "RoleId")
    VALUES (8, v_role_id), 
	(14, v_role_id), (7, v_role_id), (15, v_role_id), (1, v_role_id);
	end if;
commit;
end;
$$;
 X   DROP PROCEDURE public.add_with_role(IN user_id integer, IN namerole character varying);
       public          postgres    false            �            1259    24605 
   Categories    TABLE     |   CREATE TABLE public."Categories" (
    "CategoriesId" integer NOT NULL,
    "CategoriesName" text,
    "SoLuong" integer
);
     DROP TABLE public."Categories";
       public         heap    postgres    false            �            1259    32836 
   Permission    TABLE     w   CREATE TABLE public."Permission" (
    "PermissionId" integer NOT NULL,
    "NamePermission" character varying(500)
);
     DROP TABLE public."Permission";
       public         heap    postgres    false            �            1259    32835    Permission_permissionid_seq    SEQUENCE     �   CREATE SEQUENCE public."Permission_permissionid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 4   DROP SEQUENCE public."Permission_permissionid_seq";
       public          postgres    false    228            A           0    0    Permission_permissionid_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."Permission_permissionid_seq" OWNED BY public."Permission"."PermissionId";
          public          postgres    false    227            �            1259    32875    Posts    TABLE     �   CREATE TABLE public."Posts" (
    "PostId" integer NOT NULL,
    "TieuDe" character varying(100),
    "NoiDung" character varying(100),
    "NgayDang" date,
    "Id" integer,
    "Image" character varying(500)
);
    DROP TABLE public."Posts";
       public         heap    postgres    false            �            1259    32874    Post_PostId_seq    SEQUENCE     �   CREATE SEQUENCE public."Post_PostId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."Post_PostId_seq";
       public          postgres    false    232            B           0    0    Post_PostId_seq    SEQUENCE OWNED BY     J   ALTER SEQUENCE public."Post_PostId_seq" OWNED BY public."Posts"."PostId";
          public          postgres    false    231            �            1259    41062 	   diencuong    SEQUENCE     r   CREATE SEQUENCE public.diencuong
    START WITH 2
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
     DROP SEQUENCE public.diencuong;
       public          postgres    false            �            1259    32824    Role    TABLE     �   CREATE TABLE public."Role" (
    "RoleId" integer DEFAULT nextval('public.diencuong'::regclass) NOT NULL,
    "Name" character varying(100),
    "NormalizedName" character varying(500),
    "ConcurrencyStamp" character varying(500),
    "Id" integer
);
    DROP TABLE public."Role";
       public         heap    postgres    false    233            �            1259    41073    permissionn0    SEQUENCE     v   CREATE SEQUENCE public.permissionn0
    START WITH 21
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.permissionn0;
       public          postgres    false            �            1259    32853    RolePermission    TABLE     �   CREATE TABLE public."RolePermission" (
    "RolePermissionId" integer DEFAULT nextval('public.permissionn0'::regclass) NOT NULL,
    "PermissionId" integer NOT NULL,
    "RoleId" integer NOT NULL
);
 $   DROP TABLE public."RolePermission";
       public         heap    postgres    false    236            �            1259    32852 #   RolePermission_RolePermissionId_seq    SEQUENCE     �   CREATE SEQUENCE public."RolePermission_RolePermissionId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 <   DROP SEQUENCE public."RolePermission_RolePermissionId_seq";
       public          postgres    false    230            C           0    0 #   RolePermission_RolePermissionId_seq    SEQUENCE OWNED BY     q   ALTER SEQUENCE public."RolePermission_RolePermissionId_seq" OWNED BY public."RolePermission"."RolePermissionId";
          public          postgres    false    229            �            1259    32823    Role_RoleId_seq    SEQUENCE     �   CREATE SEQUENCE public."Role_RoleId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."Role_RoleId_seq";
       public          postgres    false    226            D           0    0    Role_RoleId_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."Role_RoleId_seq" OWNED BY public."Role"."RoleId";
          public          postgres    false    225            �            1259    32803    tangdan    SEQUENCE     p   CREATE SEQUENCE public.tangdan
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
    DROP SEQUENCE public.tangdan;
       public          postgres    false            �            1259    24612    Task    TABLE       CREATE TABLE public."Task" (
    "TaskId" integer DEFAULT nextval('public.tangdan'::regclass) NOT NULL,
    "Title" character varying(1500),
    "Decription" character varying(1500),
    "SoLuong" integer,
    "CategoriesId" integer,
    "Mark" integer,
    "DateTime" date
);
    DROP TABLE public."Task";
       public         heap    postgres    false    223            �            1259    24630 	   UserToken    TABLE     p  CREATE TABLE public."UserToken" (
    "UserTokenId" integer NOT NULL,
    "Id" integer,
    "AccessToken" character varying(1000),
    "ExpriredDateAccessToken" timestamp without time zone,
    "RefreshToken" character varying(1000),
    "ExpriredDateRefreshToken" timestamp without time zone,
    "IsActive" integer,
    "CodeRefreshToken" character varying(1000)
);
    DROP TABLE public."UserToken";
       public         heap    postgres    false            �            1259    32817    danhsach    SEQUENCE     q   CREATE SEQUENCE public.danhsach
    START WITH 3
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
    DROP SEQUENCE public.danhsach;
       public          postgres    false            �            1259    24621    Users    TABLE     y  CREATE TABLE public."Users" (
    "Id" integer DEFAULT nextval('public.danhsach'::regclass) NOT NULL,
    "UserName" character varying(100),
    "Email" character varying(100),
    "PasswordHash" character varying(500),
    "AccessFailedCount" integer,
    "ConcurrencyStamp" character varying(500),
    "NormalizedUserName" character varying(500),
    "NormalizedEmail" character varying(500),
    "EmailConfirmed" boolean,
    "PhoneNumber" character varying(20),
    "PhoneNumberConfirmed" boolean,
    "TwoFactorEnabled" boolean,
    "SecurityStamp" character varying(500),
    "LockoutEnd" date,
    "LockoutEnabled" boolean
);
    DROP TABLE public."Users";
       public         heap    postgres    false    224            �            1259    24604    categories_categoriesid_seq    SEQUENCE     �   CREATE SEQUENCE public.categories_categoriesid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public.categories_categoriesid_seq;
       public          postgres    false    216            E           0    0    categories_categoriesid_seq    SEQUENCE OWNED BY     _   ALTER SEQUENCE public.categories_categoriesid_seq OWNED BY public."Categories"."CategoriesId";
          public          postgres    false    215            �            1259    41069    permissionne    SEQUENCE     u   CREATE SEQUENCE public.permissionne
    START WITH 2
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.permissionne;
       public          postgres    false            �            1259    41071    permissionno    SEQUENCE     v   CREATE SEQUENCE public.permissionno
    START WITH 17
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.permissionno;
       public          postgres    false            �            1259    24611    task_taskid_seq    SEQUENCE     �   CREATE SEQUENCE public.task_taskid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.task_taskid_seq;
       public          postgres    false    218            F           0    0    task_taskid_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.task_taskid_seq OWNED BY public."Task"."TaskId";
          public          postgres    false    217            �            1259    24620    users_userid_seq    SEQUENCE     �   CREATE SEQUENCE public.users_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public.users_userid_seq;
       public          postgres    false    220            G           0    0    users_userid_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public.users_userid_seq OWNED BY public."Users"."Id";
          public          postgres    false    219            �            1259    24629    usertoken_usertokenid_seq    SEQUENCE     �   CREATE SEQUENCE public.usertoken_usertokenid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE public.usertoken_usertokenid_seq;
       public          postgres    false    222            H           0    0    usertoken_usertokenid_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public.usertoken_usertokenid_seq OWNED BY public."UserToken"."UserTokenId";
          public          postgres    false    221            |           2604    24608    Categories CategoriesId    DEFAULT     �   ALTER TABLE ONLY public."Categories" ALTER COLUMN "CategoriesId" SET DEFAULT nextval('public.categories_categoriesid_seq'::regclass);
 J   ALTER TABLE public."Categories" ALTER COLUMN "CategoriesId" DROP DEFAULT;
       public          postgres    false    216    215    216            �           2604    32839    Permission PermissionId    DEFAULT     �   ALTER TABLE ONLY public."Permission" ALTER COLUMN "PermissionId" SET DEFAULT nextval('public."Permission_permissionid_seq"'::regclass);
 J   ALTER TABLE public."Permission" ALTER COLUMN "PermissionId" DROP DEFAULT;
       public          postgres    false    227    228    228            �           2604    32878    Posts PostId    DEFAULT     q   ALTER TABLE ONLY public."Posts" ALTER COLUMN "PostId" SET DEFAULT nextval('public."Post_PostId_seq"'::regclass);
 ?   ALTER TABLE public."Posts" ALTER COLUMN "PostId" DROP DEFAULT;
       public          postgres    false    231    232    232                       2604    24633    UserToken UserTokenId    DEFAULT     �   ALTER TABLE ONLY public."UserToken" ALTER COLUMN "UserTokenId" SET DEFAULT nextval('public.usertoken_usertokenid_seq'::regclass);
 H   ALTER TABLE public."UserToken" ALTER COLUMN "UserTokenId" DROP DEFAULT;
       public          postgres    false    222    221    222            &          0    24605 
   Categories 
   TABLE DATA           S   COPY public."Categories" ("CategoriesId", "CategoriesName", "SoLuong") FROM stdin;
    public          postgres    false    216   w\       2          0    32836 
   Permission 
   TABLE DATA           H   COPY public."Permission" ("PermissionId", "NamePermission") FROM stdin;
    public          postgres    false    228   �\       6          0    32875    Posts 
   TABLE DATA           [   COPY public."Posts" ("PostId", "TieuDe", "NoiDung", "NgayDang", "Id", "Image") FROM stdin;
    public          postgres    false    232   ;]       0          0    32824    Role 
   TABLE DATA           ^   COPY public."Role" ("RoleId", "Name", "NormalizedName", "ConcurrencyStamp", "Id") FROM stdin;
    public          postgres    false    226   X]       4          0    32853    RolePermission 
   TABLE DATA           X   COPY public."RolePermission" ("RolePermissionId", "PermissionId", "RoleId") FROM stdin;
    public          postgres    false    230   �]       (          0    24612    Task 
   TABLE DATA           p   COPY public."Task" ("TaskId", "Title", "Decription", "SoLuong", "CategoriesId", "Mark", "DateTime") FROM stdin;
    public          postgres    false    218   O_       ,          0    24630 	   UserToken 
   TABLE DATA           �   COPY public."UserToken" ("UserTokenId", "Id", "AccessToken", "ExpriredDateAccessToken", "RefreshToken", "ExpriredDateRefreshToken", "IsActive", "CodeRefreshToken") FROM stdin;
    public          postgres    false    222   �`       *          0    24621    Users 
   TABLE DATA             COPY public."Users" ("Id", "UserName", "Email", "PasswordHash", "AccessFailedCount", "ConcurrencyStamp", "NormalizedUserName", "NormalizedEmail", "EmailConfirmed", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "SecurityStamp", "LockoutEnd", "LockoutEnabled") FROM stdin;
    public          postgres    false    220   �       I           0    0    Permission_permissionid_seq    SEQUENCE SET     L   SELECT pg_catalog.setval('public."Permission_permissionid_seq"', 1, false);
          public          postgres    false    227            J           0    0    Post_PostId_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Post_PostId_seq"', 1, true);
          public          postgres    false    231            K           0    0 #   RolePermission_RolePermissionId_seq    SEQUENCE SET     S   SELECT pg_catalog.setval('public."RolePermission_RolePermissionId_seq"', 2, true);
          public          postgres    false    229            L           0    0    Role_RoleId_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Role_RoleId_seq"', 3, true);
          public          postgres    false    225            M           0    0    categories_categoriesid_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public.categories_categoriesid_seq', 1, false);
          public          postgres    false    215            N           0    0    danhsach    SEQUENCE SET     7   SELECT pg_catalog.setval('public.danhsach', 33, true);
          public          postgres    false    224            O           0    0 	   diencuong    SEQUENCE SET     8   SELECT pg_catalog.setval('public.diencuong', 28, true);
          public          postgres    false    233            P           0    0    permissionn0    SEQUENCE SET     <   SELECT pg_catalog.setval('public.permissionn0', 131, true);
          public          postgres    false    236            Q           0    0    permissionne    SEQUENCE SET     :   SELECT pg_catalog.setval('public.permissionne', 2, true);
          public          postgres    false    234            R           0    0    permissionno    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.permissionno', 17, true);
          public          postgres    false    235            S           0    0    tangdan    SEQUENCE SET     6   SELECT pg_catalog.setval('public.tangdan', 35, true);
          public          postgres    false    223            T           0    0    task_taskid_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.task_taskid_seq', 3, true);
          public          postgres    false    217            U           0    0    users_userid_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public.users_userid_seq', 2, true);
          public          postgres    false    219            V           0    0    usertoken_usertokenid_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public.usertoken_usertokenid_seq', 205, true);
          public          postgres    false    221            �           2606    32841    Permission Permission_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public."Permission"
    ADD CONSTRAINT "Permission_pkey" PRIMARY KEY ("PermissionId");
 H   ALTER TABLE ONLY public."Permission" DROP CONSTRAINT "Permission_pkey";
       public            postgres    false    228            �           2606    32882    Posts Post_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY public."Posts"
    ADD CONSTRAINT "Post_pkey" PRIMARY KEY ("PostId");
 =   ALTER TABLE ONLY public."Posts" DROP CONSTRAINT "Post_pkey";
       public            postgres    false    232            �           2606    32858 "   RolePermission RolePermission_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public."RolePermission"
    ADD CONSTRAINT "RolePermission_pkey" PRIMARY KEY ("RolePermissionId");
 P   ALTER TABLE ONLY public."RolePermission" DROP CONSTRAINT "RolePermission_pkey";
       public            postgres    false    230            �           2606    32829    Role Role_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("RoleId");
 <   ALTER TABLE ONLY public."Role" DROP CONSTRAINT "Role_pkey";
       public            postgres    false    226            �           2606    24610    Categories categories_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT categories_pkey PRIMARY KEY ("CategoriesId");
 F   ALTER TABLE ONLY public."Categories" DROP CONSTRAINT categories_pkey;
       public            postgres    false    216            �           2606    24619    Task task_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public."Task"
    ADD CONSTRAINT task_pkey PRIMARY KEY ("TaskId");
 :   ALTER TABLE ONLY public."Task" DROP CONSTRAINT task_pkey;
       public            postgres    false    218            �           2606    24628    Users users_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT users_pkey PRIMARY KEY ("Id");
 <   ALTER TABLE ONLY public."Users" DROP CONSTRAINT users_pkey;
       public            postgres    false    220            �           2606    24637    UserToken usertoken_pkey 
   CONSTRAINT     c   ALTER TABLE ONLY public."UserToken"
    ADD CONSTRAINT usertoken_pkey PRIMARY KEY ("UserTokenId");
 D   ALTER TABLE ONLY public."UserToken" DROP CONSTRAINT usertoken_pkey;
       public            postgres    false    222            �           2606    32864    RolePermission fc_roleid    FK CONSTRAINT     �   ALTER TABLE ONLY public."RolePermission"
    ADD CONSTRAINT fc_roleid FOREIGN KEY ("RoleId") REFERENCES public."Role"("RoleId") ON DELETE CASCADE;
 D   ALTER TABLE ONLY public."RolePermission" DROP CONSTRAINT fc_roleid;
       public          postgres    false    4749    230    226            �           2606    32859    RolePermission fk_permissionid    FK CONSTRAINT     �   ALTER TABLE ONLY public."RolePermission"
    ADD CONSTRAINT fk_permissionid FOREIGN KEY ("PermissionId") REFERENCES public."Permission"("PermissionId") ON DELETE CASCADE;
 J   ALTER TABLE ONLY public."RolePermission" DROP CONSTRAINT fk_permissionid;
       public          postgres    false    230    228    4751            &   ;   x�3�L�IM.)���L�4�2���O���42�2�t�+I-*I��M�+�41������ ��=      2   i   x�-̱
�0Eѹ�!�E�ut�M��E�����򺽓�e5�xW�Ӗi� .	��G~Yhy����(.�'5j�1!��8�3XJ���b�[5�Gˋ֊�>y5�      6      x������ � �      0   �   x�mб�0�9�1(wi�fD���a`��_�R�@��Ó��a{>^uR����s�����W:O��أ8+`lQ̿���8W��ذ�k �G(z�#D8BrX��b�W;������'�[�&h��; _��QP      4   Q  x�%�˕!�(�} �_.�?��j]��I�h��Ȗ:���\m�<����|��*�v���b��RQ��|T�Bz]v2#��_dR��*V||�T)�ym�<~��re�z�ε{�I�X(-(7M(݆ҍ(�ʍ�݋`��oL�C��~�n�ƭ<������~q:ne�IP�L�����q��䑛�r��!����*5g�&n�[ �',`z��yz %==�cz$�Ȍ���A���充s�+��Ne9g�n)`�'(-d/Pb�f��%(7d9PvѰ�}���������]������f��� �����v���@���S-�~ߍ�_D�|�~�      (   �  x�mQ;R�@��S� $���!=5%��ql�xm�u�B�T��1t���I|${�ئZ�������P�cml���	�t�|���39�@Ԝ2\5G�GIڞ��1�Tpg�xk�n�}Te��
�E���k I�D�HsPZB?M��ӍM�Gj�Y\cZH��ĝ�17���9`cif�=��]�Җ�X��,�։�G3��0��TUWX�"C�D����Ǻ�q���)�!����L����YIh��uR%�J6kCn2���XS�l��܉*�N5��iJ�-�`RS>�1�9!T�J�V��˃m����B�.-��ԏ��e�=�
����CW�������;�f_�m��
��]�fq�q�%ݺ=�Zl�@�ms1�����yߍ���      ,      x��ٖ�Z��y����c�j�� �	!�0�#	N/=}�Id흁{�9�8����Z�Ax7->�f��7E��Gڌ�� .�b����B?���{��W��7R�؋N�BY��n���&l5���^\�p8����*����/�s:������%��K�͋Yv��\fo�>,��`ҹRF��>Z����oS�샥������л����	�V���"�.@���t��|os4j�����~�~�V��&v
�԰Y�W�̩�t�z�iㆽ�^�d��F9F�b?��%�S�r:7��>V݇A��:��^6ݤ��t�fl�7]]`�*�'W�S���=����� "P��wH�1xCH��P�wu�gW��/1�v���=���Dغ���G���Ik���T���Лm��l����n�sÖ}��ް:��7���s����F����p4���R��l �=�U����Ye�zc6<P���čUvZ[�Y`��տ���������c8P�D�ASծ�g�/���u[�v-1Z�XX:�x�ޜ�h�؁�+��V�	�t������3��=Έ�e���b�ߕ*1��FT�c9Δ$���� �H�]�T7K��z��/�H"b�����$���\��Nh:�n��RTuI#�#�M�PF�C�!�щ�N�U��pֵ5�i��/�ڿ�Ǫ�R'6{��$��!=��� {�ד穢��\q��@��ú����%�]�c����6p�]�G�H�8C2Ƅ�#�cBD,
rF"��8�'"�eS)i��4�$M۹���_O��ZP��!�6Bz�U ,6���ͽm�̉�E�EG����?��w �"B�u�?�4.��Qa�S�_|�\�T�9�v֕�Kuo6�;�zc��"ME�'h�Կ������B+g�W�cT�<C����'
���PW{ ��8!�8iR9B!T��d��� +r"�)�P�R*�2#z��{��T�y��'�>�n�Ɂ{��r��F������9��f{�S=D ��!	���G�����o��a�ӱ�i�4#�=Q��r��2+���|g���Hi��M㟖�v�k�N��� ->'���8a�����]�~Ȟ�w���L&
�>R"(CI�$%bٓ"&L��
�X�0d��/�|�4��|i�̸��,�i��U�2[F���C���h鹚,���I|�G �e�Z_��#HS��/��GSbx�=��2��كk{0�޴5���������4��ݞA=:ǥ5r�~U�nէtvR{h�\�?9n���˒�ߵ���O4�(��5ML#1�Y��	$@�	Y��q�HC^��6i���=iN��,�����f��x���ͩ3��bp��B&M�m[����R��F(�~� M9�7�}�3���Ry�fQX\[�d�kͶ[�)�����a��oV�ד�ӅrM>��6�l:�h���;
��=����8<*��8^S=��{��2�=X�eq�H&H`�GBBb!��I"*rIC_��6h�YN�zЄ�iO��5���a�U�H4?�x�U��̅j�^'�Ey'�M�WO��Ph��)uxAmx.>�z*�������H\N��c" h~)i��zB���A�nn��v�W�py�Imw�$s��q�Fqׅ��aP�4�g�ʜb!9�2!U�h�(B1#���@,H���wA�3��z�l����<a�8�g�R:'XOz��0�9���tq} "�Hz��W�槀�������)5�aO'��)(]�Nx>�ߘhTX_)�vz���4��N|潺�M܅�v��n�F�ќ�׮����:���8�ʯ��I�DDDDD� ��Ne�Gi��pQF�E��&f�����|�t��sў�e3Φ��=)���6�1
8Y���v���;AoR"��C������̛����I�d�,G�w|�r'&e���|8�����	ڪ٘Oȝ�ێt<��p*T��O�j���q+\�����U�t�h�@,��!E��I!Rb(�H`D�I`(ȡ���Tʤ���F~��ۤ�f����0֭,�;s��.��zӟ�'o2>�vi�n����� ��!z���@�C@�7>�	ˡ��.�
@l5��̹e|�8��_H[��g�f3��C���1� &�z{��F�&�9I���
:�(��!�=�$
 M��HDb,H���		�,&�2	���^�@�j�6h��j�� L�v�2ye������#���8�&ˊ�L�>�'-��*��qS��^U��A���6~�m2I�^}��Ls��6�na8:�Hk}�Z�<���?�Ax�Dn���c���N��U���k����.ǅ{uHC���Kр"I�������@I1��q��?,w��������s���Q���zQ��A:y!9o��t����V�^��~�u�O{�;o JX~�� M=*�2'>��AR�^��`��Ukmx��,��tr���"wb���<4�{[ys}~�]-Xd��%�t��I�X��u���i������=ΰ�^���L�#@�P R
��C!�PV����	����O��'�OA�laoq0"r��L%K�GW=�ʋfXv���iz�MQ�H_��� M�!��V�o��Z?Y�sX�h�6�ZjxǞ�tK���a���S�'zJe�U>�9��I���YuK��p��#:S��I#�z �� �'!�y�b,JGB$�H ��
Q��T�DB%T@�0Ҽ����4�&<$����8���|�K�rKfg~��ߢC׻�!h��g�@�A(�W��O!M������p��R��a�_q;yc89
�n�$8_^qs��gL"�W�V�N�򸒯��n�·�N@�;�^yΦÁ��?��g�� N`��X�b�4�,�BHe$�b2��������G���V�jͿ�4�l����r]L&dY^7@��p06�s��ݽ�����@��w��)~�@��4?�4A��0eoT[���LӬդ�8�ӯ���Yj�ϚF��N��'�q���3�J]��Mj+��;] ��F�>�l�,ޏ����w��q��y���%S5D�2�4�R!L�"$�3EN\Ӽ���O&|��i�6�-�+a�u�Z�N��hF�d��QuRsN�vR=���7��ʞ~
i��r�(px7;d��ICA���%�}�q���ڧ/�4�49x��Q��Xd3MH͗�Qk��r19���T,�P� 鏤!��LB�:	'��)QD�X ��aDe��Q��D4������Z[}B�@��xC?2I�O��8�[Wa��Kg��A.�#/t��� �3� �Fe����������#�1��c�̙�q�4�3�κ`��ɬ���y����	�Q��2^~�������`V:�݊��排*�5��tJ7��[��ng"��_ב��:B&E�@��$D� G
�
�dI�WO��o��o��	ɓ�9.���t�|EN�rґ�x�ϼ�h|��3/L��C�[��#�;b���������0!�;fk8`"^��o� ~�1��TVC�syo����3Vɭ��z�dp���� �/�xZ�Q�G�"m��m��v�s��w��{��H�O<% E4�X ��i"�1T)a�	SYC�8�j�6g��r��uu�1�6��tK�e��϶;��nJ-���D��y��8C�w�߈"�
z�� ��~��R���2�Xk� $PgL��
S���ɇf����7#;�g�&�����Ӟ����p�Ky>��x7\q�d�5��B>>��H<�(�(�2%iT�$��F䄤|��(4�(�F�W{�����v�O7cp�{����c5�A�ݍl�dB/RI~T4�4��T���;�Ҕ�*p:��j��tF^΁��"p�}��W��l�z����I��q���畿�}�w�D'��Mօ���$����i�&���i~ř|����`e!i$�`!Ji,D��PI�4���W��I�����Is�{^��}�\Fp&����F{�D��NX�r(��Y�H��!}!��=�����/gL���pt`{&o�+����-��6��lu�����@    ���m�'ӌ���%A'�%s���X���</��6[���V� �ޖLIC�3	#^�����I"J,)Ƃ�P,�
����������4,���I�@T&�+;6��3w �F�kO���=I��a�+|&��$�H�u�?4~kr	Ӯ�<K��:�����]TPr;����︷�Xw��gLq�󘉋�S2W�I�ܾ������GG[��$ڮE�3hx�a!� �a�d���)e�S&B&� �D��(�f4�����襩5}�&�^׏W�y>8y�!YwT{�7W�ee�������?�F~�,}�@y�E�Д���\`�~e8���V�j�$�5h8>2�Q�`r��䋉'��}F��Z��8��rz��v̱��N�P����q���:��I�Ə���a&BzD�Y���B�X��J#g!DE������z��h�b,��ɶ�����s�p�s8�nx�g͡��ʴ����4���W�槐�Ӳ�897*�Z�JC,F�\o���nj��X">�����O����Yِځ�X��~�M�a^v��:T�j��'Gu�pK>��ǙH�{�`La*S!"���T���
"��B�La�y��4�3��O����ǡ.�E�p|A���>̵����جw��B^����|�I|G�1|�y�!��50��1���U����4�{3���l�k�>l�����3L=��n���x��鸝�U?5vi7L�Y��R��H,��J���w@�q&I�1��(YJD!�C,�R!dH���D���ѫ=�OH�>�k�؆���4��{�{�/�@��iɹL(re_�
B�z|&&�Y^k�~i�Y��#�z�Ǹ���v��L3L�@ӳJ���to��4�3��nRD�K=>k��c^��p"N��l:�PLw���*��g���3z7&Y"�a��� ˄"�q,9�������/��A��~���nwy�5=ZF� l�>�\_�ko�O�{�^kB��}�M���J�{�T`���t:�c�o��|]6}3��0M6�Ѯ�.,d5e����J���v<��Q�-:��?�K�(}E
�:���#���Gަ�k��P ~�I�q#7��!F�A�����QYGưS�(m���q����Qi���q�s ���`����luV����~�ې�W;�f��<�r���rr��(�<L*�H:ѡ6Z-z�wQ/K���� �|�2	�˙X��T�{@ �9�L2,$$�"�6�T������ �2��	��X����(��γ ?����Q����:r���v��叐�x˦"cI~A�@F'>�_�4=�Z�&M��V'{0ژ�~��Xe��4��W�yΜ�Gx���I�Z��C�w p�ż�s��I�N9��ҵG�p��7��2����DQ���܏FR9J�@�0����Řo2�aO���1���I�?Xst��ᬗ�c\j�v֙����8�'O��
�Pxm�u���3�%'~�&`���/c�4M���'��#�����7mL'�����1�)k��4Oz���c�8���B��`݌j�4G�3���p���g�� � �2�8�Xz�F�H%!D,y����A1�D��oB�ԛ'@&����n;AFf�H=��F֥�8^�ڐ���D�%����19!2R��*�9�'gK��v7,K�7K p>i���9�|Ʋ��|lquT��d�S���t��TSg�xVH9�G�鵊�\(�{������#d��D�%��$H(S��R�R&	QA�E�B
P� ��oA�\�<2��`f9�l�әЁ�~��6��a��'�Y����i��X��Y��2 �d�g0F��_v��V����4��LO~i��`�,j��9�
�2nc9Op1t��+��BY��� xu�s&�A���u[�eYm��c�H�S�U�I�D#n���eK�B���1&͈cY����Ř�1�������o�d�E�:%�h�m�a��^���m-�ɭm�����߸���2?2�����[�l#�-���V]����K����n6&����[}B_V�3�X&6��2T�n��ܫ�r/��b3[����3ʂ�2܊��n���J�d!��X�SM� S9R9d�(�J�c���7!�V�(���ͦ�����sYo��
�R��hR�K;ч"oq}{2�r�3�+S�g;�6G���L�7O�l�7�o�t���d4�4n4b�Ia���eM_A��0�fzB���^�M>�P�{���K�q:�z2�3�օ��2�d���)�"9!��Y&(B��	i�H��)d�A/�|2���O�Lw�N����!<�3��$c�2Q���-��H_��R2�;�� A�� �6G�d� S�u]3�R�匏Ԧ��������־��4S��.���>�:�*6�`��;ͮ(2g��sFD��j�Y���t�o
cA&�p%#)���
ID�d@dIJ,�a�$�"�.�:�ۏW��l���{2��h����2h*�l��n�]�}uh�t)W.QǱ����Y!˗^3?����o�k+hx&4�]'�� 	<>�d"�8p*b"����0���gt����q�9�ҡޞ�r)S,/�.Bqq�ٓep���si�G�H��L��eq��PR�S39#ANC�II�"�Ti^���#M����N��x���d�`�]\��\�7�\��x�#�y���iZ�{��	�k������ M�7�j��W{�ߍ����>4Ai�&�j��ި�����|�y����C�6H��~���-;�7��d ��Y��eڮ�߄3{��_��B�[�"��($ G�8d�B��'����'N����$N�6�I�є�����X�<.S)v��L'�lG�(�eu���x�V�D��*���)���;�0s�F<q����|j�Y�T2�S��'�����ۘe|5����Gj�����4��ji��z�=ј�ݩ�P���3d~��}�A�0!)d���
J�@�Oԉ1i�`��)��L����K�܂��i�,���H��Y��!�S������r�T����7QVX��:�!gj����:�m�-L��m�mn\v&gj��ƾiO[등�3������u��X���geN�.f^����^���� +�i�q&A ��Fa��X�r��9��e
F���h$F�W����1U�rƵ��dۛ�]k��e��MXtHU���8��Ȩ�k�3g{�Ȣ�����os�Ͼ��Ya�>�]����qgz�氶�ņI��/��$g�Y>Aά��M���o����o�t�&�O�.�q3.���>C�d��b�2�JJ�����R$!��T�d�fLꈿ�̫����)�ϸ��t��>����i��ؓDG�"n�[�&�R�4���?�f(x�&g^G� ��x���j ��)��U�d�>1��u���zQ�{c"���7K}�h>'�"�\iU����b����c\��9u��J7�6��G�(� Cߗ�B� %�R� ��P�E�K����fi�A�e�휩����Lǀ����K�	{����y1�pN��n�i��xz���3����MoD���p?%e�i�j�����%�1��oR1����U_�*G�������Vg�	�'�L�y\6X;4�|�2Ҷ�~$��Ę�B1�Ϸ��^��[�)��`,�dB���%���
ā�r���B!J �S�#��B�j����ן�25E�̎�����v#�����b���2�l5�Nc��g�`�(��g��ۜ�3&��nN�!����1y��eO����Z�{�>������Z����4F��������\�c��ep��(��j����>~f�1�ǲ��1b�9dʷ��0"I"��0�P g$Ic^6��3e����>�?l�j�w��vm|��Co��[�{/���q}�6Q�p�(�0��,c���쟡f�)j��N�����^ �9��c�	�6(�2ńU|6�[�>�3
�1:�Zg=���K'E�Z*���j�w��^D���1�j�A���D�2H�LH1/ g1n��X�3	�8    �i^���R3,�J<A�l�~��L��E�q�'=W�����ބC���:�.��r��>s�7,*������H�\���vq˷���}ܯ�l�"V7w�6G���`�E��pk�'4�S>�å喉<Z�w��\ލ��̄�]�J2;W�!�+Ș�a���P�Y������VdIHE(ǼJ#'1�~A�;�ab�9�L�n�k���\�TP�;i9�P�N���Q�~�z	K����2AA�u�? 2���S���kUPV����ĕ�=XZ-w]̩�r�?}���|�-Ө��}��G4��+J2�x=��HL��]�'�E��͓���-B�Wg^F$)E,�L!QR �)N�X1�� B�3��z���3�G��A�6����t���j�Y� ���Fv5��N��;�s�e�=kV$�J�i��3�+Y���p�*�=��I�P˛��ǯ+�n����nu�$�Sz���+Id�uk���۳Fۦ:\�s0*��x����A{$��Nx�9F���0�q
�(�$�x	
M��H��$�7ܒW�7��t��g��s�;C���m�u��vTj��n����mP���Y�z�%LѾI ���4�����K-u��9���Y�T��s����j��'��&k�u��3�7��-�M��v¾/f�u�m����\I-�����d�,~��`���d�*2_9�B�"��E*���4E!b�a,�D
c� �r��d�i�3�f3�@���q��#sJdG��?Y��l�u�����Y�KS� �Ϥ���@��2���n|̀}mk��5O~m^-�����Ө���B_eL�2�	-3$�?��.j6:oVnHU����5e�TzlΣ��[���}),Ę����ʢ� �$d����@b�B*J*J!#��J���0�WK}�xv�Ւ�'��;g4��a�7����k2��f�_wݺ�&�c0��!L��Zf~H�4���;��+���w�󭶕�M9=�hm;���Č�
닄�R+������&$�s�n�d6��f��t�Y�.��7cO�kջh�h�r�3	��g"4�	�A�D��&�E���
L��("�4�4�&|���u��\�y���]�jә��k䫎^�=:����4w�q���L����ɤ�l�Rǖ����na��o����*G�>˼MuJ,''��M2�8��3J3+�c��(v��|a{GhNz��o���t9�����s9���e5)�!'��"�n@*��)H��B�ő�h 2�e�����'�y�����dD��x��ğQ��u^d�~�-$�BIox����4��ْB%�i~ijkmգ_mk8�k�A�|>hP���/�V7�U� P;����4OY��޼}�ɚ����#������p鏒�Yق��e��<~$��+�D"q��b�b
�F#�(�����bJ�f��q�E���Ƕ�{�򼫪�v����8�W�J��y^��p�F�i�5�݆�i���7�����Pi\�&��=+��I��2�p+XXm�Ϥ�`V[H�YeR�_������xB��V����Q�-Y��°���D����Ҧ��P�gD��M�a
H,�P"IŐ7�B��$�B(J1f�y5�41��Q�1T{��MWK+q����ri�ʘ�t�#��%���m���;.�!�4��Pi��K��w��L�4��^٣�rcx}F��!J���0:�g�i�hD1���h�\Q�C�m$.��A�q��ܼ#��g�/����,ΰ|o�C�$&i�a&i(���AI���Y�z�滠ɯ����������O������-Ͱ?���7w!�-|j2�/��4L���gЫL�C@Sk�oG�����3��4M0�k˩����V=��v]����_�ۊ�O(�\��6��� �8|��`�_�r~��#=V"�9��ʍ�>�F�Ǚt���E�,
�"���k� eb��r�Hf�y��3�z�3��3u��I��r�k��9/Ց�kW���6�8=���[\��g�œ�&��W�:ԟ��Ҽj�x�MR�\�Ll5p��Oöj���*,oT~��	l��H�|9�='k�*V�R=H� �lM��0�fc�+�Q���'�M,_�� ���� EqƯ�%!�e*���cG4�!#ͫ'�ۤ��O�
őn��M�*��d3�.Fݕ5�������{j����H"�ʟ�����ɤ�ә�~���3W4&�y��Ա�l���`�>�1n��+#M�<���y<�+e7F��_V�1������kS���-�7;O��i�}�@�	�q
�!#H��2�(
�K��H��I�a�)����H�mҰ@p�z�L�P���Ta�|��W�e/��[>F�e}V���z���4�=n�Q$�e��SH�f���E��h3}Ûi6A�i��oz�jkc�:�o�ȝlGC�H��w�V�^3��7Pϲه/���[�N1Gu�g+?j��8�L�ܷi�2��H bi҄i�
	�E4�Ϟ^���'Mk>��I�/,��l����|d������]nw{3�wO�I��7�)/���B�E��{S��'��[Ut`�`�׌4Φ��i�:0��/.�lG�=�J��S�꧳w�/m?��'��c�Vv���cj���>G(e�i�ʼ���8�"@��A,D�E�4J#�#3Ҽ��O��<ap�.����Q}�Me���t1MWH�gu1����d��=�i���H�϶���ٚ�����K-�OT�W��i�11�Q�Uk�د�k�:��#�	�I��$���Ȅ&���d�ɮ���a X��,�O��X&�3��x���HB�P$	@!�%Y@I�`9��0��4��e�d�O�4űM�^��4��p9
g�[�6��z睇~h��z���mz��]��w,�aE��5��3H����"Pu´�Z�w�s൅\�g�}�Q�֨��6�����}FE8�h�d��F�׋�֝+�	]��+��QV�d_4�oq����,�d����$�$�(A�D2B)��P�C%
�c/Ui^��&�y}�V�n�תkX�"��:̜��u��N�Z�YKC��H���7@dB^��� M�]-ī��%"&��}�}�7���;G�|N7���og��aN����L����*�y���ѳ���,����I*�%�F��4\>�8)F��J ED�� ɔD����Dp,!1�pLR�O(����F�Hz5�����B��l��Ii�1#ʹ�[̫�W�]��ГYZ_4?�4U�����畹���YG�M|u j<��V���aqF�VY*eC(H I!(d!1�sa� ,U�7�"Zނ�>�*�3�c�/�Cw��º���Á��$���xC����K8�8C�w��0K���:�`+����s����E��I٘ ��f�ll��_�f������G)T��dFS��IϞC�}e?�/��������W�)"�x�L]	�����I%CX�e���*ILd�Ws���ޞ���Ɲy-��T3�ৡ�V�1�0��>*�0)6Ӡ��Z��w,3A+����3��f�m'a������bI��`r�=�<�����@�]�X�N�q�t�zZ8�����,�3�n�_w�'9C�b���v��>��3�|�q&�Ǚ|/�$̢T�b���TP�(h*E"̔0������m���~�&'-�1,�8������*���~�����9E���} A��JVd���~i�[P.*SMJ�1����`�fL�8Zc8Uk�]����U��l��H���M{�x4�Z�Mv3]��rn*�q�gQvSGS��~��0����L��wI��H��q�w@Q�R��e$�q�0м����71a�[�d_���)�ߊ���bW��c�)g�@ܘ��Z��x�oC������7	P
^��?4ݵ��,C�0�L��8hL�sIS��̩��tk�q�Y���XS�����4�������P�P����ԁ98OK&E~�ho�ݿ�D4<�0��N ����X 2�N{��N"_� R��,��0м����e�OX�t��K��    ����|�_�u`�y�S��))�s��7Kc;����i!|���W��M=*}�[Y������4��uPvn��;�$(����
���6bo�g�s�]��^��f��݊���"ϗ¾��噰u�6�v�-���}���JD��Q�D�E!%|	n)B�(�"��T�B�������y�d�R3��f�\��n0�fyg���������D��sx���g��ʄ�/E�3@SV�v:�L)S/W���p�qnQk��Fe��7�ti0К�{w�<e�r��~^�Q1���v�	H����L�2�eĊlS �|�4�8�@�	�"*@�EP@���,��(Nc�d�$D�����m�ĭU>��f�?�����z�rf9)�Ŕ�ž�Ϭބ.�t�++?\��@��B(f����3H�$��Z��w�!��S�����um8>1��N��ʄ���m�/9�aJ����G�&��2,��D��B�L��x;aB���m��yH�=�$��r�4�	o�#�A�	2�3A�aAL����Wo�I�3�?a
azXKBvS��������vM��� 9`�`#?�ݻ�E$I�K����L5K~Ь2�M�ﶭcOF�5K��`�#�1��೥��*�ޟP�)�7GZKiTs3*�||V���G���q~/������ �qF���5"(�8I%y��B�PHQ��@ThL$F�Wo��I���	��N)�q���4ohj�wU�W�����zֶ�F6��=i���;o
�2xi��A���:�|1���;�c��V�d�S������U��߻m��A�J�lq�^7��#���!�YV2�譽����9L���4���L��}�JB`F���a9��$��(� �c
Q�'�^���%o0/�'ؔkZ�p;^�� �i�#RD���^���>�l���p�� _7O?�4�UXe�eG��p��̓�`c6��n]�h���,��V���y'l�2��=�G�xq�lf�ڞ�l7��Q����������4q�L�~�3���x�aI��/�DA	b>��b!R($�^��"�Xa�y��4�S�<�s��	�<��QO��[����u���Z%m�,��R�Hs�%�&�K�u�?�4��eL.�V�L�P�n��@Æc��7ٔ�5�4�%i�)K�,�����=�V�X$M2�|Sk�v0V^\n%(�#i�K�D�`��$b*�$dss1�n0I1�$��������O!�`��O�B��t�-к��#=ڤ��R;��7a���j�kU��"|T4���&a��۷�6G��)�Q�{�J�/�h�Z������XkqJ��V�]ێ��a�j�Z�	v���^*1[N�KY�mfe��RK��0�!X�zN4� D~��>�F�Q"�S�8e�E)�8��%r�=���m9�3�����=_� t�,�W��<��l*�zD���@����.X�q)?`����T2R�l���t�O�3:�K�Ƥ
1���-{.f������(wLj�k�eO��%l,��P�Q�dv�* э�@�������5��^��I��y��@,��(����i(�ġ@R	!H� e+rJ F�?k�B���/��!(w���sP�_���,8�z���V�%>�KuǇ��]�;,��qQ�i����+<�z(��)�ٓ�?#���P�mM&^5��-��i~��M��	�H���|b�Mr_$N6ߪ���=�8�LUYr���N=o�J�������N&��~�#�6�:�řx�sdq&*�~�-S���^EwҰ
&TS%	�L���H����~:��ק��z��jklקhh]�e����N˽�/O�u�-��h�Co����)*H�ݘ��_�Zi��e.d2������A�
�,"~e>(����� ��#��\�ſ}�v
�����f&H��hƩ?lP8X�xw���d���,���︡��*���g��f����D~�4������{�͜�;91<�����@P��sO��PO���(��-p�#_�>���u���Lr���B�tf�zd����K5�\�C�bI潿X��C HQ!�P����g��$�P#���g����IO�̲���\Π�^�����r�R��b
6�:�i��![��R>Xh@Fk���%������:H~G�HD�Ù���~��p�����:����[o�=�����V�M���іi��_�k�>π���}>�X ��Ee�9���d�:5k�׌.m��\#̑�>�F��Z�d�q4)+w��T�X�b�.͆��z)��p�y'7���zn�����(��@ �.� �H�8�bY�B��?����{I/�NGZ����wocW�c�@k��'3�v�ߟ�R�<�S���k���g�\�jwm!����)�� ؞��yPj'�����ro܅�K�Z����f�-�Ǖ�gNwN��r��e|�&�!��9A��j�<��+�+��!�*QR"(�2WhQ�0�x^M�߭�ȴ�'ԍG�l�>�7��(��8��f����p��2�WA�7�}��2�7"c�j"�!u�[���T�\#�_��6|����Z�zm�M0�Zk`R��'�J=�n�ǳXmaб�W�VL�����9<V�l�*���_:��H�W�����A�I�L�2`�"��Y�E�4	1�����D�m�����'Mu���e`˒�|8��ΘX��uP�@� ��ٔ�n��^t} �)2�����B�dm�\�č��M��EPSpm::#� _�>����S	�cMo�W��0��~�npN/N�V=Y:�{pMn���kf)��Pp�3E"��J
�0Je�$r�r)$ ThS ���A����V5h�O��B�'Ӄ�ي`8n��� �Wm�}]ғ�8�^:�w|�8��;�o�(�e/�C83m�R#A�C����`�K���2�����(�c��4>q���y��b��x7��� ]%�#�:Y��?f$#Ŀ�$A���0��+�״d�$r�8"Q�C%SB,b9c�y�4�|���q����,��_��`�kt[f�^]��9�9vw8aI~эw��>�	C��Lhj���۞Uά0є_���U/~�d:��w�&P���R4���R;���g�['XS<<����C_���p��uv�Ct� ��֩����=̨� ��I�HJ̲�'I	1�㘔f!EL�$4/��o�F�;��z�DB��B���n�[�1 �d���W�����A�����a�?��=m�7S��!�)sj9yx~{�b�v~�N=�:�>2��F1J�m���HC�$j�a�)TӃ���l�цaoyV��y��A���C��<T�����qFz�)2�7J3,(Yx�!E�ʒ$e�� i^m��'��O �o�8ii� ���(?��Z�j}5�Ǿ6:-7e/(.��ޭ_gD��4?�4��M{f#nfn{|��_�=�^�c�ʬ���Fu�_l��ћg�F�$]$iu��f�N��.m�u�Qg땱?�qy�H��KX�!����dO6$��+L�@�Ȓ�c��8�Ҍo��_��'da���MGJꕧ.����%���2�q��)�y�l�6$�=(,o~�� ���k����pt◼�ݸ�pPj���`XeP��+�OF�)zF+Ww�\�l����:O��2��R��8�L����.V�#i�=ΠL�&�q�#E@r$$�|A$��LD1����_&��'K��P��\���x�����5��B�7����6p��輺��<jto$'"&�5��3HSv�_�k������\^�A|��=��g���j}j�U��=�g���R����W{�㦶��땧�/@k�;�������G�`|�1<�ý�����ݓ���$J�tP���_���ڽ����b}�=<,���ٝq��{����46<Θ��2��0�*� J$�ȑ�*a$���\�DE!'�s��Ic���FI��32ˁ�?-��u=��}�;<w�At��+�<167��4�'�b���>�uҬ6bҞ��+ݎ�a�q�b�'\�����9�<�m�� �  � �>{�i�"r�r���Z���ۀB��h�����^���z����I��q���V���1�%H
JI�e��J��Ȟ�S�?Od6�t�I�~��x��9�yg��-�w�jw9��]��N��9?�<�c�L���=�� Maa_kQ_[�ړ�
�M��M�iL�u���rh�-ĳ�ߓ�v`K�n��I�]�v*���4��"��l�&7g��Zg'�*�_'?r���rC�0	��,j��<qb�D c�J*4V)2~'�� �y�p�ۀA8H�,��Y:nw����Y�U���j�YfL@�H���=K|�dFmh��ao6� q�4�*��%�U��z�y�����8e��f	�0#�d���jY�\z�(1Z�J���@]�k��z��n<������I���@5���bQ厤 eT��(�D�4O��gIc/�Y7`>��[ԗ:,w3�T]��&+�Ԟ��b�l��\���0��ڕ��� �W[���CmXӈy�N�kݵh�6E+B��4��_Ow��s���9�+�c��S��%n� </�'�u}�6(��m�}['�k�+h詿lM���߮��4������8��{J���0�XH��I�HR"�4� P������!�9�@S���qig��~���>�{�yiǼ�h��3w2�k���Sw�K���/XV�D��~�dN�ey=����dO�9�τ�o�1{�·��DN�����ײ��SE�|�g��d��=�?ӗf��M:��b�q��o��q����<Θ
���P�X)~Ϟ������$M`	&�4O��Ió'���sG�t^��չXz�kL�����(Qi���x�k �/ ~��7�y�i�	h�an��9ս�R�f�vS�W�=�Er�e�K��"�AS/ak��߼sW�//%�콽4���nֵc�4kZ���fX��aF��a��nJ�s�H
��A+$Q���s���4g,�����L7�%�Z�<����������z�Z�*Nt?��
8�AH4�0���q�*_�*�^����ኦ}�:7ֺ9ϧrS� ʺBQ�q�������<ED#v��]zgb��A�H/}ۄFIN��R%g]�f~� ����Qq��!���)4b�c��D	dT��A��?hڞ��p��]Uc{�ևf��=Nܳ�:��]Y<gc�#h��c�<��X�oO7c1
�^F&�4^�qИu����*:س�����-h,��u�d����e_b׭i���ŭ�ņ��L��㡛l�Gа{�ɈQM�1ϑ����gM$<kb��B�KQ��I�IC��???�SD2�O�8E�v�y��ٱ6K�����=_oW�ײ?7���kM���|č���!Y��M�v����k��J��]���;�}z3�62�D���!�7���7�
�8U��sÅQ<�2;��4ʼ��l�˖�Kwf<pJ�w����T���0�*�XJT�JDE�"�Ea��G���9O��ō�x&������41�u�
�r8s�ZP��Ș^�y�����N��?�"��7�y/�]��x�d�¿��>ˢ��m�;�"��\�d��D��c�����&\5�����ڍnzO�.���_��s|0�q�^�de`u��@B�qƨا�ō��@�IJ���Ȣ�&�"�8i�N�O�ƻ�칬����%�%����������w���m!�0.G-��P�$ւ=�{�&�*���F���jS}�@��+�^��tף&�6���<$����4Y�jd�l;w�Ӓ���[��g��r���J���d��o�f���i��8S(f�!5��XR%p,����T�y�FQ*��4O��WH�DO����*�O�7=����1ZT�|����r���-�����0'�/�0B���=HSt��	�~&�Q���%�Z��,t���^�{bߥ���i��U7q3<���䜘�WE]�ۥ�p5�N���J������GҼ����%Q�$���A�D5��T�	��������<i��I@�D}ý1�Qg0��׉�zP.�BVG��d���ċ�����	�9E\ժ�'��b�7!�Sy���5CL�Y�w����n[,����^�F݁�,i��IH[��4�)��Ln_�v��1&�����*�7É�㇫a��{T"��B�Q�`̕L�I��DR(#R� GcHCN�_��W���D��?1Q�?�
����㏶`�������V`�ڡ�l���rZ�0�Z��՝��[�����d4E�Z��<�f�%�EWw�]y���k�f��̵�r��˳��+�+���3��-�v�a[��m	�6ۛ]r��~�r����
'���d��(C���tz D�pC2�o�UCA�HI��DXJ��	�S�p�����OK����xz�:C�Ϝ߶W�?��G��ګ�CY'���=(E�(q�N��>m6�C������ �ĉ��'����8l�ӵn�<#����=s�qh�7Z������K�t*]�=���Y�7���9m_�dW���:�i�|�3UH\�TET��$�H���}J*�D%J �$���jj�(���k�G&�Z���N���c��A����:�W8�-n��O��t�ڿ���F�s����v�=�D�;����<��0t7YT��Y|�����,�?��g���pϏ��p��a�k�����"�L����n�ǚ�����9<׫ר�Ӆ��ƍl�hI�rn�J�Fc෦�9�Vw�^��i���w-������p
R��4�!`��5-Pf�'X2�=)�A�`�p���^��ܪ�#wN��[��;{��d�?ԭ�5�ް��¥k$��_���x	�_��}mݟt�����3��#��W9��F�3���\�m�R�2�8���7�sb�����-��"�7W0��{q�gt��+�/����A�q�y���XM)�*'HIňJq�b� ��r��}��^���~M�������;����¿6��?�Ëǩ̺�n��5�G�uM��9%�(GH��ƃd��N��2:ك���G�'d/*bD����O=���c�u�W7:G
O�����=�Usר}{��ټ0�$������h��C��7G/�U�ݣ]I�h���z�wo�b4��翪kY�Z"�(�}Q*�D�LB�!O�D�<�	�R��
�C��{ͬӀ�X�8�ma6e<�n��lU��M�����u���7�/�K_��\��<ই�S�����W�}�Ͱ0fpe"qT��n�ّ�UZ"�GF��c/;����ih��-�;���i�%wT�6��|K���V�T�	v@%X"a
��E��j���  ����������n�:�Ź�2
S��5�,G)����&کZ���aM�]$�>R�
1��y��"�*:�aJS��ڨ��U�žm��Q97�0J�6�ߴ8��&H�t�NZ�E�?�ju�vgw4r9ꇷe{9��ʣ��H��8S��V  �2P��X��8��ǒ���H?���Ǐ��e      *   �  x�œݎ�@��ǫ��S�0��-E0�� ?�2�.P�j��^Ǧ�U�4M[<�=i�d2�2y�<��,�o_)��������z}H��,��!����d����X�r3uz� �_�Y���E�Y8��-�C`77So.��uҍ97�/(:�����ual�����S��ɀE`ݡ����RH�ef%�1���P��s-O����?z�іOm�H��4��M.�ɹm���Ә���~�ݝ�,x�&)�eo_r��p�P�V�	.�
����[y4�^D�	��c���,|y9z�C-c�Oi}�CBO����v%s2�E���G���3�H�q��"B1�����Z���O�$��į.9�,^�O��DO�<>D�l:Z��ռ(���mڢ�[���[YӇ����j��Eu�O5@�Uu>�q5�>���%w�0���g'�6|+���9moi|i���\�X�4��n��5!7!��G�O�XQ)D��\�-=�!�Z_     