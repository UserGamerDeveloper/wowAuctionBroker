local frame = CreateFrame("Frame")
local factionId;
local realmId;
local action = {
		["PLAYER_LOGIN"] = function() playerLogin() end,
		["UPDATE_FACTION"] = function() setReputation() end,
		["PLAYER_MONEY"] = function() setMoney() end,
		["TOKEN_MARKET_PRICE_UPDATED"] = function() setTokenPrice() end,
		["TOKEN_REDEEM_RESULT"] = function() print("TOKEN_REDEEM_RESULT") end,
		["TOKEN_SELL_RESULT"] = function() print("TOKEN_SELL_RESULT") end,
		["TOKEN_BUY_RESULT"] = function() print("TOKEN_BUY_RESULT") end,
	}
frame:RegisterEvent("PLAYER_LOGIN")

frame:SetScript("OnEvent", function(self, event, arg1)
	action[event]();
end)

function playerLogin()
        -- Our saved variables, if they exist, have been loaded at this point.
        if DB == nil then
            -- This is the first time this addon is loaded; set SVs to default values
            DB = {}
        end
				local const = {
			["Kazzak"] = 215,
			["Mal'Ganis"] = 195,
			["Twisting Nether"] = 241,
			["Ravencrest"] = 232,
			["Blackmoore"] = 153,
			["Antonidas"] = 134,
			["Azjol-Nerub"] = 147,
			["Silvermoon"] = 234,
			["Tyrande"] = 167,
			["Hyjal"] = 214,
			["Ревущий фьорд"] = 213,
			["Свежеватель Душ"] = 235,
			["Гордунни"] = 210
		};
		realmId = const[GetRealmName()];
							local Zandalari_Empire = 2103;
local Proudmoore_Admiralty = 2160;
local englishFaction, localizedFaction = UnitFactionGroup("player");
if englishFaction == "Horde" then
	factionId = Zandalari_Empire;

else
	factionId = Proudmoore_Admiralty;
end
		if not DB.realmsDatasByIdHouse[realmId].moneyMax then
			DB.realmsDatasByIdHouse[realmId].moneyMax = GetMoney();
		end
			setMoney();
			setReputation();
			setTokenPrice();
		 frame:RegisterEvent("PLAYER_MONEY")
		frame:RegisterEvent("UPDATE_FACTION")
		frame:RegisterEvent("TOKEN_MARKET_PRICE_UPDATED")
		frame:RegisterEvent("TOKEN_REDEEM_RESULT")
		frame:RegisterEvent("TOKEN_SELL_RESULT")
		frame:RegisterEvent("TOKEN_BUY_RESULT")
end

function setMoney()
		DB.realmsDatasByIdHouse[realmId].money = GetMoney();
		if DB.realmsDatasByIdHouse[realmId].money > DB.realmsDatasByIdHouse[realmId].moneyMax then
			DB.realmsDatasByIdHouse[realmId].moneyMax = DB.realmsDatasByIdHouse[realmId].money;
		end
end

function setTokenPrice()
		local a,b,c = C_WowTokenPublic.GetCommerceSystemStatus();
		if a then 
			DB.tokenPrice = C_WowTokenPublic.GetCurrentMarketPrice();
		end
end

function setReputation()
	local name, description, standingId, bottomValue, topValue, earnedValue, atWarWith,
  canToggleAtWar, isHeader, isCollapsed, hasRep, isWatched, isChild = GetFactionInfoByID(factionId);

		 DB.realmsDatasByIdHouse[realmId].reputation = bottomValue;
end