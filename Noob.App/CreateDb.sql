CREATE TABLE "EquippedItems" (
    "UserId" INTEGER NOT NULL,
    "SlotId" INTEGER NOT NULL,
    "ItemId" INTEGER NOT NULL,
    CONSTRAINT "PK_EquippedItems" PRIMARY KEY ("UserId", "SlotId")
);


CREATE TABLE "Items" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Items" PRIMARY KEY AUTOINCREMENT,
    "SlotId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Price" INTEGER NOT NULL,
    "Level" INTEGER NOT NULL,
    "Attack" INTEGER NOT NULL,
    "Defense" INTEGER NOT NULL,
    "Sneak" INTEGER NOT NULL,
    "Perception" INTEGER NOT NULL
);


CREATE TABLE "UserItems" (
    "UserId" INTEGER NOT NULL,
    "ItemId" INTEGER NOT NULL,
    CONSTRAINT "PK_UserItems" PRIMARY KEY ("UserId", "ItemId")
);


CREATE TABLE "Users" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY AUTOINCREMENT,
    "BrowniePoints" INTEGER NOT NULL,
    "Niblets" INTEGER NOT NULL,
    "Experience" INTEGER NOT NULL
);


CREATE TABLE "UserCommands" (
    "UserId" INTEGER NOT NULL,
    "CommandId" INTEGER NOT NULL,
    "ExecutedAt" TEXT NOT NULL,
    CONSTRAINT "PK_UserCommands" PRIMARY KEY ("CommandId", "UserId"),
    CONSTRAINT "FK_UserCommands_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);


CREATE INDEX "IX_UserCommands_UserId" ON "UserCommands" ("UserId");


